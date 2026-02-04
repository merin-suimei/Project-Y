using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Splines.Interpolators;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWalker : Enemy
{
    public EnemyWalkerPatrolState walkerPatrolState;

    [SerializeField] private EnemyWalkPoint[] enemyWalkPoints;
    public EnemyWalkPoint currentEnemyWalkPoint { get; private set; }
    public override EnemyState patrolState { get; protected set; }

    private Coroutine pointStayCoroutine;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new EnemyWalkerPatrolState(this, stateMachine, "IsMove");

    }

    protected override void Start()
    {
        base.Start();
        if (enemyWalkPoints.Length > 0)
        {
            agent.SetDestination(enemyWalkPoints[1].transform.position);
            currentEnemyWalkPoint = enemyWalkPoints[1];

        }
        stateMachine.Initialize(patrolState);
    }
    public override void Update()
    {
        base.Update();
    }

    public void ExecutePointStay(float waitTimeOnPoint)
    {
        if (pointStayCoroutine != null) { 
            StopCoroutine(pointStayCoroutine);
        }
        pointStayCoroutine = StartCoroutine(PointStayRoutine(waitTimeOnPoint));
    }
    private IEnumerator PointStayRoutine(float waitTimeOnPoint)
    {
        stateMachine.ChangeState(idleState);
        yield return new WaitForSeconds(waitTimeOnPoint);
        if (stateMachine.CurrentState == idleState) { 
            stateMachine.ChangeState(patrolState);     
        }
    }

    public void InterruptStay()
    {
        if (pointStayCoroutine != null)
        {
            StopCoroutine(pointStayCoroutine);
            pointStayCoroutine = null;
        }
    }

    public EnemyWalkPoint GetNewEnemyWalkPoint()
    {

        List<EnemyWalkPoint> availableWalkPoints = new List<EnemyWalkPoint>();

        foreach (var point in enemyWalkPoints)
        {
            availableWalkPoints.Add(point);
        }

        for (int i = availableWalkPoints.Count - 1; i >= 0; i--)
        {
            if (availableWalkPoints[i].transform.position == currentEnemyWalkPoint.transform.position)
            {
                availableWalkPoints.RemoveAt(i);
                break;
            }
        }

        if (availableWalkPoints.Count == 0)
        {
            availableWalkPoints.AddRange(enemyWalkPoints);
        }


        int randomIndex = Random.Range(0, availableWalkPoints.Count);
        EnemyWalkPoint newWalkPoint = availableWalkPoints[randomIndex];

        return newWalkPoint;
    }


    public void SetEnemyWalkPoint(EnemyWalkPoint nextWalkPoint)
    {
        currentEnemyWalkPoint = nextWalkPoint;
        agent.SetDestination(currentEnemyWalkPoint.transform.position);
    }

}
