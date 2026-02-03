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
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemyWalker.IsPlayerVisible())
            enemyWalker.stateMachine.ChangeState(enemyWalker.detectState);

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

            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                isWalkPointSet = false;
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
