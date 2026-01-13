using UnityEngine;

public class EntityState 
{
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        //anim.SetBool(animBoolName, true); 
    }

    public virtual void StateUpdate()
    {

    }

    public virtual void Exit()
    {
        //anim.SetBool(animBoolName, false);
    }
}
