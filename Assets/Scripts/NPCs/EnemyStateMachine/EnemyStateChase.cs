public class EnemyStateChase : EnemyState
{
    public EnemyStateChase(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (enemy.IsPlayerChaseable())
            enemy.agent.SetDestination(enemy.player.position);
        else
            enemy.stateMachine.ChangeState(enemy.patrolState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
