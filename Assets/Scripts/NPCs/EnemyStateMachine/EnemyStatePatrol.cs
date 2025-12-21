using UnityEngine;

public class EnemyStatePatrol : EnemyState
{
    bool isWalkPointSet = false;
    Vector3 currentWalkPoint;
    public EnemyStatePatrol(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerReachable())
        {
            enemy.stateMachine.ChangeState(enemy.detectState);
        }

        if (!enemy.isWalkPointSet)
        {
            currentWalkPoint = enemy.GetNewWalkPoint();
            enemy.agent.SetDestination(enemy.currentWalkPoint);
            isWalkPointSet = true;
        }

        if (enemy.agent.remainingDistance <= 0.1f)
        {
            isWalkPointSet = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
