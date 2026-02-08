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

    [SerializeField] private EnemyWalkPoint[] enemyWalkPoints;
    public EnemyWalkPoint[] EnemyWalkPoints => enemyWalkPoints;

    [SerializeField] private bool isPatrolPathClosed;
    public bool IsPatrolPathClosed => isPatrolPathClosed;

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


}
