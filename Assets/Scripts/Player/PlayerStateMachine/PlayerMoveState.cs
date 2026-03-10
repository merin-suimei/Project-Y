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
        EventBus.Raise(EventType.PlayPlayerFootStepSound);
        //player.cameraMain.Priority = 0;
       
    }
    public override void StateUpdate()
    {
        base.StateUpdate();

        if (player.moveDir.sqrMagnitude < 0.1f)
        {
            stateMachine.ChangeState(player.idleState);
        }

        //Старое управление
        //player.SetVelocity(new Vector3(player.moveDir.x*player.MoveSpeed, player.rb.linearVelocity.y, player.moveDir.z * player.MoveSpeed));

        //Новое управление
        float moveSpeedFactor = player.GetMoveSpeedFactor();
        float animSpeedFactor = player.GetAnimSpeedFactor();

        player.animator.speed = animSpeedFactor;
        player.SetVelocity(new Vector3(
            player.moveDir.x * player.MoveSpeed * moveSpeedFactor,
            player.rb.linearVelocity.y,
            player.moveDir.z * player.MoveSpeed * moveSpeedFactor
        ));
    }

    public override void Exit()
    {
       // player.animator.SetBool(animBoolName, false);
        player.animator.speed = 1f;
        base.Exit();
        EventBus.Raise(EventType.StopPlayerFootStepSound);
    }

}
