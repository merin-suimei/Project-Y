using UnityEngine;

public class EnemyWalkerPatrolState : EnemyState
{
    private bool isWalkPointSet = false;
    private float timer;
    private Quaternion targetRot;
    private float speedRot = 5f;
    EnemyWalker enemyWalker;

    public EnemyWalkerPatrolState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyWalker = (EnemyWalker)enemy;
    }

    public override void Enter()
    {
        base.Enter();
        isWalkPointSet = false;
        Debug.Log("Enter patrol");
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemyWalker.IsPlayerVisible() && enemyWalker.IsPlayerChaseable())
        {
            enemyWalker.InterruptStay();

            enemyWalker.agent.ResetPath();
            enemyWalker.stateMachine.ChangeState(enemyWalker.detectState);

        }

        if (!isWalkPointSet)
        {
            enemyWalker.SetEnemyWalkPoint(enemyWalker.GetNewEnemyWalkPoint());
            timer = enemyWalker.currentEnemyWalkPoint.waitTime;
            targetRot = enemyWalker.currentEnemyWalkPoint.transform.rotation;
            isWalkPointSet = true;
        }

        if (isWalkPointSet && enemyWalker.agent.remainingDistance <= 0.1f)
        {
            enemyWalker.transform.rotation = Quaternion.Slerp(enemyWalker.transform.rotation, targetRot, Time.deltaTime * speedRot);

            if (Quaternion.Angle(targetRot, enemyWalker.transform.rotation) <= 1f)
            {
                enemyWalker.ExecutePointStay(timer);
                isWalkPointSet = false;
            }

        }

    }

    public override void Exit()
    {
        base.Exit();
        isWalkPointSet = false;
        Debug.Log("Enter patrol");
    }
}
