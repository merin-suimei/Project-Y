using UnityEngine;
public class EnemyDetectState : EnemyState
{
    float detectDelay;
    float detectProgress;
    float decaySpeed;
    private bool isRotatedToPlayer;
    private Vector3 detectedPlayerPos;
    private float detectSpeedRot = 10f;

    public EnemyDetectState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}


    public override void Enter()
    {
        base.Enter();

        detectDelay = enemy.detectDelay;
        detectProgress = 0f;
        decaySpeed = 1f;
        isRotatedToPlayer = false;
        detectedPlayerPos = enemy.player.position;
        enemy.agent.isStopped = true; 
        enemy.agent.updateRotation = false;
        enemy.agent.ResetPath();      
        enemy.agent.velocity = Vector3.zero;
        EventBus.Raise(EventType.TurnOnEnemyPattern, enemy);
        EventBus.Raise(EventType.PlayEnemyDetectSound);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        RotateToPlayer(detectedPlayerPos);

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
        {
            EventBus.Raise<Enemy>(EventType.TurnOffEnemyPattern, enemy);
            enemy.stateMachine.ChangeState(enemy.patrolState);
        }
        else if (detectProgress >= detectDelay)
            enemy.stateMachine.ChangeState(enemy.chaseState);
        
    }

    private void RotateToPlayer(Vector3 playerPos)
    {
        Vector3 directionToPlayer = (playerPos - enemy.transform.position).normalized;
        directionToPlayer.y = 0;

        Quaternion targetQuat = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetQuat, Time.deltaTime * detectSpeedRot);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.agent.updateRotation = true;
        enemy.agent.isStopped = false;
        // EventBus.Raise(EventType.OnEnemyExitDetect);
    }
}
