public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyModel enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemy.IsPlayerVisible)
            enemy.stateMachine.ChangeState(enemy.detectState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

