public class StateMachine
{
    public EntityState CurrentState { get; private set; }
    public void Initialize(EntityState initState)
    {
        CurrentState = initState;
        CurrentState.Enter();
    }

    public void ChangeState(EntityState newState) 
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
