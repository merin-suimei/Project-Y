public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyModel enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
        EventBus.Raise(EventType.EnemyEnableChaseSpeed, enemy.id, true);
        EventBus.Raise(EventType.PlayEnemyMoveSound);

        // Не проверяем ID чтобы остановить всех мобов
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, PlayerCaught);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        EventBus.Raise(EventType.OnMoveTo, enemy.id, enemy.player.position);
    }

    private void PlayerCaught()
    {
        enemy.stateMachine.ChangeState(enemy.patrolState);
    }

    public override void Exit()
    {
        base.Exit();
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, PlayerCaught);

        EventBus.Raise(EventType.EnableEnemyPattern, enemy.id, false);
        EventBus.Raise(EventType.EnemyEnableChaseSpeed, enemy.id, false);
        EventBus.Raise(EventType.StopEnemyMoveSound);
    }
}
