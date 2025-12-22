public class EnemyStatePatrol : EnemyState
{
    private bool isWalkPointSet = false;

    public EnemyStatePatrol(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemy.IsPlayerVisible())
            enemy.stateMachine.ChangeState(enemy.detectState);

        if (!isWalkPointSet)
            enemy.SetWalkPoint(enemy.GetNewWalkPoint());

        if (isWalkPointSet && enemy.agent.remainingDistance <= 0.1f)
            isWalkPointSet = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
