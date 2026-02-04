//using System.Numerics;
using UnityEngine;
public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
        enemy.agent.speed = enemy.chaseSpeed;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        /*if (enemy.IsPlayerChaseable())
            enemy.agent.SetDestination(enemy.player.position);
        else
            enemy.stateMachine.ChangeState(enemy.patrolState);*/
        enemy.agent.SetDestination(enemy.player.position);
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) < 1.5) 
        {
            EventBus.Raise(EventType.OnEnemyCatchPlayer);
            enemy.stateMachine.ChangeState(enemy.patrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        EventBus.Raise<Enemy>(EventType.TurnOffEnemyPattern, enemy);
        enemy.agent.speed = enemy.patrolSpeed;
    }
}
