using System.Collections;
using UnityEngine;

public class EnemyModel : IModel
{
    public readonly int id;

    public EnemySoundsData soundsData { get; private set; }
    public float detectDelay { get; private set; }
    public float decaySpeed { get; private set; }

    public StateMachine stateMachine { get; private set; }
    public EnemyIdleState idleState { get; private set; }
    public EnemyChaseState chaseState { get; private set; }
    public EnemyDetectState detectState { get; private set; }
    public EnemyState patrolState { get; protected set; }

    public Transform player { get; private set; }
    public EnemyWalkPoint[] enemyWalkPoints { get; private set; }
    public bool isPatrolPathClosed { get; private set; }

    public bool IsPlayerVisible { get; private set; } = false;
    private Coroutine pointStayCoroutine;

    public EnemyModel(int id, EnemyType type, EnemyWalkPoint[] enemyWalkPoints, bool isPatrolPathClosed, EnemySoundsData soundsData)
    {
        this.id = id;

        this.soundsData = soundsData;

        this.enemyWalkPoints = enemyWalkPoints;
        this.isPatrolPathClosed = isPatrolPathClosed;

        detectDelay = type.DetectDelay;
        decaySpeed = type.DecaySpeed;

        stateMachine = new StateMachine();
        idleState = new EnemyIdleState(this, stateMachine, "IsIdle");
        chaseState = new EnemyChaseState(this, stateMachine, "IsMove");
        detectState = new EnemyDetectState(this, stateMachine, "IsDetect");
        patrolState = new EnemyPatrolState(this, stateMachine, "IsMove");

        player = GameManager.instance.player.transform;

        stateMachine.Initialize(patrolState);

        EventBus.Subscribe<int, bool>(EventType.OnPlayerVisible, SetPlayerVisible);
    }

    public void Destroy()
    {
        EventBus.Unsubscribe<int, bool>(EventType.OnPlayerVisible, SetPlayerVisible);
        stateMachine.CurrentState.Exit();
    }

    public void Tick()
    {
        stateMachine.CurrentState.StateUpdate();
    }

    private void SetPlayerVisible(int targetID, bool value)
    {
        if (targetID != id) return;

        IsPlayerVisible = value; 
    }

    private IEnumerator PointStayRoutine(float waitTimeOnPoint)
    {
        stateMachine.ChangeState(idleState);
        yield return new WaitForSeconds(waitTimeOnPoint);
        if (stateMachine.CurrentState == idleState) { 
            stateMachine.ChangeState(patrolState);     
        }
    }

    public void ExecutePointStay(float waitTimeOnPoint)
    {
        if (pointStayCoroutine != null) { 
            GameManager.instance.ProxyStopCoroutine(pointStayCoroutine);
        }
        pointStayCoroutine = GameManager.instance.ProxyStartCoroutine(PointStayRoutine(waitTimeOnPoint));
    }

    public void InterruptStay()
    {
        if (pointStayCoroutine != null)
        {
            GameManager.instance.ProxyStopCoroutine(pointStayCoroutine);
            pointStayCoroutine = null;
        }
    }
}
