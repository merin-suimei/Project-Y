using UnityEngine;

public class EnemyStatic : Enemy
{
    public override EnemyState patrolState { get; protected set; }
    protected override void Awake()
    {
        base.Awake();

        patrolState = new EnemyStaticPatrolState(this, stateMachine, "IsPatrol");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(patrolState);
    }

    public override void Update()
    {
        base.Update();
    }
}
