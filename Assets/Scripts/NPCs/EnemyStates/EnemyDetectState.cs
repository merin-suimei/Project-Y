using UnityEngine;
public class EnemyDetectState : EnemyState
{
    float detectDelay;
    float detectProgress;
    float decaySpeed;

    public EnemyDetectState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}


    public override void Enter()
    {
        base.Enter();

        detectDelay = enemy.detectDelay;
        detectProgress = 0f;
        decaySpeed = 1f;
        EventBus.Raise<Enemy>(EventType.OnEnemyEnterDetect, enemy);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (enemy.IsPlayerVisible())
        {
            detectProgress += Time.deltaTime;
            EventBus.Raise<float>(EventType.OnEnemyLoseAim, detectProgress);
        }

        else
        {
            detectProgress -= Time.deltaTime * decaySpeed;
            EventBus.Raise<float>(EventType.OnEnemyLoseAim, detectProgress);
        }

        if (detectProgress <= 0)
            enemy.stateMachine.ChangeState(enemy.patrolState);
        else if (detectProgress >= detectDelay)
            enemy.stateMachine.ChangeState(enemy.chaseState);
        
    }

    public override void Exit()
    {
        base.Exit();
       // EventBus.Raise(EventType.OnEnemyExitDetect);
    }
}
