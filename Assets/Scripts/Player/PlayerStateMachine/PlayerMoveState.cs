using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetRotationAllowed(true);
        //player.cameraMain.Priority = 0;
       
    }
    public override void StateUpdate()
    {
        base.StateUpdate();

        if (player.moveDir.sqrMagnitude < 0.1f)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.SetVelocity(new Vector3(player.moveDir.x*player.MoveSpeed, player.rb.linearVelocity.y, player.moveDir.z * player.MoveSpeed));
    }

    public override void Exit()
    {
        base.Exit(); 
    }

}
