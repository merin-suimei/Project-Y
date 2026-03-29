using UnityEngine;

public class BabaYagaChaseState : BabaYagaState
{
    bool isHidden;
    public BabaYagaChaseState(BabaYagaModel model, StateMachine stateMachine, string animBoolName)
        : base(model, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
        isHidden = true;
        EventBus.Subscribe(EventType.OnTimerIsUP, Spawn);

    }

    private void Spawn()
    {
        isHidden = false;
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        if (!isHidden) 
        {
            EventBus.Raise(EventType.OnMoveTo, model.id, GameManager.instance.player.transform.position);   
        }

    }

    private void PlayerCaught()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
       

    }

}
