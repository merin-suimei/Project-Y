public class EnemyPatrolState : EnemyState
{
    private EnemyWalkPoint currentPoint;
    private int pointIndex;
    private int directionIndex;

    public EnemyPatrolState(EnemyModel enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, ResetPoint);
    }

    ~EnemyPatrolState()
    {
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, ResetPoint);
    }

    public override void Enter()
    {
        base.Enter();
        currentPoint = enemy.enemyWalkPoints[pointIndex];
        EventBus.Raise(EventType.OnMoveTo, enemy.id, currentPoint.transform.position);
        EventBus.Raise(EventType.PlayEnemyMoveSound);

        EventBus.Subscribe<int>(EventType.OnMoveToArrived, EnemyOnPoint);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (enemy.IsPlayerVisible)
        {
            enemy.InterruptStay();

            enemy.stateMachine.ChangeState(enemy.detectState);
        }
    }

    private void ResetPoint()
    {
        pointIndex = 0;
        currentPoint = enemy.enemyWalkPoints[pointIndex];
    }

    private void EnemyOnPoint(int targetID)
    {
        if (targetID != enemy.id) return;

        EventBus.Raise(EventType.OnRotateTo, enemy.id, currentPoint.transform.position + currentPoint.transform.forward);
        CalculatePointIndex();
        enemy.ExecutePointStay(currentPoint.waitTime);
    }

    private void CalculatePointIndex()
    {
        if (enemy.isPatrolPathClosed)
        {
            pointIndex++;
            if (pointIndex >= enemy.enemyWalkPoints.Length)
            {
                pointIndex = 0;
            }
        }
        else
        {
            pointIndex += directionIndex;
            if(pointIndex >= enemy.enemyWalkPoints.Length - 1)
            {
                pointIndex = enemy.enemyWalkPoints.Length - 1;
                directionIndex = -1;
            }
            else if (pointIndex <= 0)
            {
                pointIndex = 0;
                directionIndex = 1;
            }
        }
    }

    public override void Exit()
    {
        EventBus.Unsubscribe<int>(EventType.OnMoveToArrived, EnemyOnPoint);
        base.Exit();
        EventBus.Raise(EventType.StopEnemyMoveSound);
    }
}
