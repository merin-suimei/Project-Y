using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetRotationAllowed(true);
        player.SetVelocity(new Vector3(0f, player.rb.linearVelocity.y, 0f));
        player.IsometricCam.gameObject.SetActive(true);
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        if (player.moveDir.sqrMagnitude > 0.1f)
        {
            stateMachine.ChangeState(player.moveState);
            return;
        }

        Vector3 currentVelocity = player.rb.linearVelocity;

        Vector3 targetVelocity = new Vector3(0f, currentVelocity.y, 0f);

        Vector3 smoothedVelocity = Vector3.Lerp(
            currentVelocity, 
            targetVelocity, 
            player.AccelerationRate * Time.deltaTime
        );

        player.SetVelocity(smoothedVelocity);
    }

    public override void Exit()
    {
        base.Exit();
        player.IsometricCam.gameObject.SetActive(false);
    }

}
