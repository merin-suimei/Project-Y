using UnityEngine;

public class EnemyStaticPatrolState : EnemyState
{
    private EnemyStatic enemyStatic;
    private float timer;
    private int stepIndex = 0;
    private float waitTime = 2.0f; 
    private float speedRot = 2f;
    private float[] rotationSteps = { 0f, -45f, 0f, 45f };

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    public EnemyStaticPatrolState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyStatic = (EnemyStatic)enemy;
        spawnPosition = enemy.transform.position;
        spawnRotation = enemy.transform.rotation;
    }

    public override void Enter()
    {
        base.Enter();
        timer = waitTime;
        enemy.agent.SetDestination(spawnPosition);
        stepIndex = 0;
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemyStatic.IsPlayerVisible())
            enemyStatic.stateMachine.ChangeState(enemyStatic.detectState);

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            float targetAngle =  rotationSteps[stepIndex];
            RotateToAngle(targetAngle);

            Quaternion targetQuat = spawnRotation * Quaternion.Euler(0, targetAngle, 0);
            if (Quaternion.Angle(enemyStatic.transform.rotation, targetQuat) < 1.0f)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    stepIndex++;
                    if (stepIndex  >= rotationSteps.Length)
                    {
                        stepIndex = 0;
                    }
                    timer = waitTime;
                }
            }

        }
        
    }

    private void RotateToAngle(float angle)
    {
        Quaternion rotTarget = spawnRotation * Quaternion.Euler(0, angle, 0);
        enemyStatic.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotTarget, Time.deltaTime * speedRot);
    }
    public override void Exit()
    {
        base.Exit();
    }

}
