public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected string animBoolName;
    //protected Animator animator; animator will be later
    protected Enemy enemy;
    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.animBoolName = animBoolName;   
    }

    public virtual void Enter()
    {
        //enemy.animator.SetAnimation(0, animBoolName, true); 
    }

    public virtual void StateUpdate()
    {
        
    }

    public virtual void Exit()
    {
        //enemy.animator.SetAnimation(0, animBoolName, false); 
    }
}
