using UnityEngine;

public class PlayerStuckState : PlayerState
{
    public PlayerStuckState(Player player, StateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetRotationAllowed(false);
        player.TeleportToCheckpoint();

    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit(); 
    }
}
