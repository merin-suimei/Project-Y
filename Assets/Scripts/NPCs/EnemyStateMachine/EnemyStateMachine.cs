using UnityEngine;

public class EnemyStateMachine
{
    EnemyState currentState;
    public void Initialize(EnemyState initState)
    {
        currentState = initState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState) 
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }
}
