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
            return;
        }
        
        //Старое управление
        //player.SetVelocity(new Vector3(player.moveDir.x*player.MoveSpeed, player.rb.linearVelocity.y, player.moveDir.z * player.MoveSpeed));

        //Новое управление
        float moveSpeedFactor = player.GetMoveSpeedFactor();
        float animSpeedFactor = player.GetAnimSpeedFactor();

        player.animator.speed = animSpeedFactor;

        Vector3 targetVelocity = player.moveDir * (player.MoveSpeed * moveSpeedFactor);
        Vector3 currentVelocity = player.rb.linearVelocity;
        targetVelocity.y = currentVelocity.y;
        Vector3 smoothedVelocity = Vector3.Lerp(
            currentVelocity, 
            targetVelocity, 
            player.AccelerationRate * Time.deltaTime
        );
        player.SetVelocity(smoothedVelocity);

        player.stepsPlayer.LaunchRandomSound();
    }

    public override void Exit()
    {
       // player.animator.SetBool(animBoolName, false);
        player.animator.speed = 1f;
        base.Exit();
        EventBus.Raise(EventType.StopPlayerFootStepSound);
    }

}
