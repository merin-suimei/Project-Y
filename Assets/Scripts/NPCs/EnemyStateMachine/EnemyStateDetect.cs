using UnityEngine;
public class EnemyStateDetect : EnemyState
{
    float detectDelay;
    float detectProgress;
    float decaySpeed;

    public EnemyStateDetect(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();

        enemy.ShowDetectImage(true);

        detectDelay = 1.5f;
        detectProgress = 0f;
        decaySpeed = 1f;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (enemy.IsPlayerVisible())
            detectProgress += Time.deltaTime;
        else
            detectProgress -= Time.deltaTime * decaySpeed;

        if (detectProgress <= 0)
            enemy.stateMachine.ChangeState(enemy.patrolState);
        else if (detectProgress >= detectDelay)
            enemy.stateMachine.ChangeState(enemy.chaseState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.ShowDetectImage(false);
    }
}
