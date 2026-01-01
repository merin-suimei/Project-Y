public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
