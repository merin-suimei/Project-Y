public class EnemyState : EntityState
{
    protected EnemyModel enemy;
    public EnemyState(EnemyModel enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        EventBus.Raise(EventType.OnAnimationStart, enemy.id, animBoolName);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        EventBus.Raise(EventType.OnAnimationStop, enemy.id, animBoolName);
    }
}
