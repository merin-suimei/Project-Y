using System;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private EnemyWalkPoint currentPoint;
    private int pointIndex;
    private int directionIndex;
    private SoundEmitter soundEmitter;
    public EnemyPatrolState(EnemyModel enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, ResetPoint);
    }

    ~EnemyPatrolState()
    {
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, ResetPoint);
    }

    public override void Enter()
    {
        base.Enter();
        currentPoint = enemy.enemyWalkPoints[pointIndex];

        soundEmitter = SoundManager.Instance.Get().Initialize(enemy.soundsData.walkSoundData);
        soundEmitter.Play();

        EventBus.Raise(EventType.OnMoveTo, enemy.id, currentPoint.transform.position);
        EventBus.Raise(EventType.PlayEnemyMoveSound);

        EventBus.Subscribe<int>(EventType.OnMoveToArrived, EnemyOnPoint);
        EventBus.Subscribe<int>(EventType.OnRotateToArrived, EnemyTurnedOnPoint);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemy.IsPlayerVisible)
        {
            enemy.InterruptStay();

            enemy.stateMachine.ChangeState(enemy.detectState);
        }
    }

    private void ResetPoint()
    {
        pointIndex = 0;
        currentPoint = enemy.enemyWalkPoints[pointIndex];
    }

    private void EnemyOnPoint(int targetID)
    {
        if (targetID != enemy.id) return;

        EventBus.Raise(EventType.OnRotateTo, enemy.id, currentPoint.transform.position + currentPoint.transform.forward);
    }

    private void EnemyTurnedOnPoint(int targetID)
    {
        if (targetID != enemy.id) return;

        CalculatePointIndex();
        enemy.ExecutePointStay(currentPoint.waitTime);
    }

    private void CalculatePointIndex()
    {
        if (enemy.isPatrolPathClosed)
        {
            pointIndex++;
            if (pointIndex >= enemy.enemyWalkPoints.Length)
            {
                pointIndex = 0;
            }
        }
        else
        {
            pointIndex += directionIndex;
            if(pointIndex >= enemy.enemyWalkPoints.Length - 1)
            {
                pointIndex = enemy.enemyWalkPoints.Length - 1;
                directionIndex = -1;
            }
            else if (pointIndex <= 0)
            {
                pointIndex = 0;
                directionIndex = 1;
            }
        }
    }

    public override void Exit()
    {
        try
        {
            soundEmitter.Stop();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to stop soundEmitter in id " + enemy.id + "\n\n" + e.ToString());
        }

        EventBus.Unsubscribe<int>(EventType.OnMoveToArrived, EnemyOnPoint);
        EventBus.Unsubscribe<int>(EventType.OnRotateToArrived, EnemyTurnedOnPoint);
        base.Exit();
        EventBus.Raise(EventType.StopEnemyMoveSound);
    }
}
