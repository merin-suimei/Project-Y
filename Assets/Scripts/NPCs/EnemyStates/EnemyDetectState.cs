using UnityEngine;
public class EnemyDetectState : EnemyState
{
    float detectProgress;

    public EnemyDetectState(EnemyModel enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();

        detectProgress = 0f;
        EventBus.Raise(EventType.OnInterruptMoveTo, enemy.id, true);
        EventBus.Raise(EventType.EnableEnemyPattern, enemy.id, true);
        EventBus.Raise(EventType.PlayEnemyDetectSound);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        EventBus.Raise(EventType.OnRotateTo, enemy.id, enemy.player.position);

        if (enemy.IsPlayerVisible)
        {
            detectProgress += Time.deltaTime;
            EventBus.Raise(EventType.OnEnemyLoseAim, enemy.id, detectProgress);
        }

        else
        {
            detectProgress -= Time.deltaTime * enemy.decaySpeed;
            EventBus.Raise(EventType.OnEnemyLoseAim, enemy.id, detectProgress);
        }

        if (detectProgress <= 0)
        {
            EventBus.Raise(EventType.EnableEnemyPattern, enemy.id, false);
            enemy.stateMachine.ChangeState(enemy.patrolState);
        }
        else if (detectProgress >= enemy.detectDelay)
            enemy.stateMachine.ChangeState(enemy.chaseState);
    }

    public override void Exit()
    {
        base.Exit();
        EventBus.Raise(EventType.OnInterruptMoveTo, enemy.id, false);
        EventBus.Raise(EventType.EnableEnemyPattern, enemy.id, false);
    }
}
