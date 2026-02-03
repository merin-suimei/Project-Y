using UnityEngine;

public class PlayerState : EntityState
{
    protected Player player;
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {

    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void Exit()
    {

    }
}
