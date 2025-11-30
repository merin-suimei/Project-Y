using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("")]
    [Tooltip("Range to detect possible collision")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float detectionDistance = 3f;
    [Tooltip("Semicon angle for detection (in degrees)")]
    [UnityEngine.Range(0f, 90f)]
    [SerializeField] private float detectionSemiconeAngle = 45f;
    [SerializeField] private Transform enemyEye;



    [SerializeField, Tooltip("Patrol points")] private Transform[] walkPoints;
    [SerializeField] private LayerMask playerMask;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 currentWalkPoint;

    private bool isWalkPointSet;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        player = GameManager.instance.player.transform;
        if (walkPoints.Length > 0)
            agent.SetDestination(walkPoints[0].position);
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (IsPlayerVisible() || dist <= detectionDistance)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 dir = (player.position - enemyEye.position).normalized;

        if (Vector3.Angle(enemyEye.forward, dir) > detectionSemiconeAngle)
            return false;
        if (Vector3.Distance(enemyEye.position, player.position) > detectionRange)
            return false;
        if (Physics.Raycast(enemyEye.position, dir, out RaycastHit hit, detectionRange))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
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

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        DrawVisionConeGizmos();
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360f, detectionDistance);
    }
    #endif

    private void DrawVisionConeGizmos()
    {
        if (enemyEye == null) return;

        Vector3 leftRay = enemyEye.position +
            Quaternion.Euler(0, detectionSemiconeAngle, 0) *
            (enemyEye.forward * detectionRange);

        Vector3 rightRay = enemyEye.position +
            Quaternion.Euler(0, -detectionSemiconeAngle, 0) *
            (enemyEye.forward * detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(enemyEye.position, leftRay);
        Gizmos.DrawLine(enemyEye.position, rightRay);
    }


}