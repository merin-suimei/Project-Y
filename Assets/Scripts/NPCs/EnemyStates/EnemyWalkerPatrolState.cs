using UnityEngine;

public class EnemyWalkerPatrolState : EnemyState
{
    private bool isWalkPointSet = false;
    private Quaternion targetRot;
    private float speedRot = 5f;
    private EnemyWalker enemyWalker;
    private EnemyWalkPoint currentPoint;
    private int pointIndex;
    private int directionIndex;

    public EnemyWalkerPatrolState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyWalker = (EnemyWalker)enemy;
    }

    public override void Enter()
    {
        base.Enter();
        currentPoint = enemyWalker.EnemyWalkPoints[pointIndex];
        enemy.agent.SetDestination(currentPoint.transform.position);
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


        if (enemyWalker.agent.remainingDistance <= 0.1f)
        {
            Quaternion targetRot = currentPoint.transform.rotation;
            float timer = currentPoint.waitTime;
            enemyWalker.transform.rotation = Quaternion.Slerp(enemyWalker.transform.rotation, targetRot, Time.deltaTime * speedRot);

            if (Quaternion.Angle(targetRot, enemyWalker.transform.rotation) <= 1f)
            {
                CalculatePointIndex();
                enemyWalker.ExecutePointStay(timer);

            }

        }

    }

    private void CalculatePointIndex()
    {
        pointIndex += directionIndex;
        if(pointIndex >= enemyWalker.EnemyWalkPoints.Length - 1)
        {
            pointIndex = enemyWalker.EnemyWalkPoints.Length - 1;
            directionIndex = -1;
        }
        else if (pointIndex <= 0)
        {
            pointIndex = 0;
            directionIndex = 1;
        }
    }

    public override void Exit()
    {
        base.Exit();
        isWalkPointSet = false;
        Debug.Log("Enter patrol");
    }
}
