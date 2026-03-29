using UnityEngine;

public class BabaYagaState : EntityState
{
    protected BabaYagaModel model;
    public BabaYagaState(BabaYagaModel model, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.model = model;
    }

    public override void Enter()
    {
        base.Enter();
        EventBus.Raise(EventType.OnAnimationStart, model.id, animBoolName);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        EventBus.Raise(EventType.OnAnimationStop, model.id, animBoolName);
    }
}
