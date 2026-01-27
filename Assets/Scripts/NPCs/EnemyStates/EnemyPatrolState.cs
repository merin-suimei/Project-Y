using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private bool isWalkPointSet = false;
    private float timer;
    private Quaternion targetRot;
    private float speedRot = 5f;

    public EnemyPatrolState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemy.IsPlayerVisible())
            enemy.stateMachine.ChangeState(enemy.detectState);

        if (!isWalkPointSet)
        {
            enemy.SetEnemyWalkPoint(enemy.GetNewEnemyWalkPoint());
            timer = enemy.currentEnemyWalkPoint.waitTime;
            targetRot = enemy.currentEnemyWalkPoint.transform.rotation;

            isWalkPointSet = true;
        }

        if (isWalkPointSet && enemy.agent.remainingDistance <= 0.1f)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * speedRot);

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
