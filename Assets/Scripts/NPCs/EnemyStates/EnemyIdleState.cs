using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemy.IsPlayerVisible() && enemy.IsPlayerChaseable())
            enemy.stateMachine.ChangeState(enemy.detectState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}

