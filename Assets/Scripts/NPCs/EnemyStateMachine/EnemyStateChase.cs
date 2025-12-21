using UnityEngine;

public class EnemyStateChase : EnemyState
{
    public EnemyStateChase(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base .Update();
        enemy.agent.SetDestination(enemy.player.position);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
