using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float sightRange;
    [SerializeField, Tooltip("Точки для патрулирования")] private Transform[] walkPoints; 
    [SerializeField] private float detectingTimer;
    [SerializeField] private float maxDetectingTimer;
    [SerializeField]private LayerMask playerMask;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 currentWalkPoint;

    private bool isWalkPointSet;
    private bool isPlayerDetected;
    private bool isChasePointSet;




    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform; //пока что взяла трансформ так, потому что нет скрипта игрока
    }

    void Start()
    {
        detectingTimer = maxDetectingTimer;
        currentWalkPoint = walkPoints[0].position;
        isWalkPointSet = true;
        //берем здесь из game manager игрока  player = GameManager.Instance.player.transform; 
    }

    
    private void Update()
    {
        isPlayerDetected = Physics.CheckSphere(transform.position, sightRange, playerMask);

        if (isPlayerDetected)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!isWalkPointSet)
        {
            currentWalkPoint = GetNewWalkPoint();
            agent.SetDestination(currentWalkPoint);
            isWalkPointSet = true;
        }

        if (agent.remainingDistance <= 0.1f)
        {
            isWalkPointSet = false;
        }
    }


    private void ChasePlayer()
    {
        detectingTimer -= Time.deltaTime;

        if (!isChasePointSet) { 
            agent.ResetPath();
            agent.SetDestination(player.position);
            isChasePointSet = true;
        }

        if (agent.remainingDistance <= 0.1f)
        {
            isChasePointSet = false;
            //входим в состояние атаки
        }

        if(detectingTimer <= 0)
        {
            Debug.Log("ChasePlayer");
            detectingTimer = maxDetectingTimer;
        }

    }

    private Vector3 GetNewWalkPoint()
    {

        List<Transform> availableWalkPoints = new List<Transform>();

        foreach (var point in walkPoints)
        {
            availableWalkPoints.Add(point);
        }

        for (int i = availableWalkPoints.Count - 1; i >= 0; i--)
        {
            if (availableWalkPoints[i].position == currentWalkPoint)
            {
                availableWalkPoints.RemoveAt(i);
                break;
            }
        }

        if (availableWalkPoints.Count == 0)
        {
            availableWalkPoints.AddRange(walkPoints);
        }

        
        int randomIndex = Random.Range(0, availableWalkPoints.Count);
        Vector3 newWalkPoint = availableWalkPoints[randomIndex].position;

        return newWalkPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }

}
