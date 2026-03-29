using UnityEngine;

public class BabaYagaModel : IModel
{
    public readonly int id;
    public StateMachine stateMachine { get; private set; }

    public BabaYagaChaseState chaseState { get; private set; }
    public Transform player { get; private set; }

    public BabaYagaModel(int id)
    {
        this.id = id;
        stateMachine = new StateMachine();
        chaseState = new BabaYagaChaseState(this, stateMachine, "IsMove");
        stateMachine.Initialize(chaseState);
    }

    public void Tick()
    {
        if(stateMachine.CurrentState != null)
            stateMachine.CurrentState.StateUpdate();
    }
}
