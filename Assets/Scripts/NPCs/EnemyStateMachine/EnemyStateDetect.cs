using UnityEngine;
using System; 
public class EnemyStateDetect : EnemyState
{
    float detectingTimer;
    float detectImageTimer;
    public EnemyStateDetect(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        detectingTimer = 3.0f;
        detectImageTimer = 0.5f;
    }

    public override void Update()
    {
        base.Update();
        detectingTimer -= Time.deltaTime;
        if (enemy.IsPlayerReachable() && detectingTimer <= 0) 
        {
            enemy.ShowDetectImage();
            detectImageTimer -= Time.deltaTime;
            if (detectImageTimer <= 0) 
            {
                enemy.HideDetectImage();
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
