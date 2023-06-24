using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int impactHash = Animator.StringToHash("Enemy Impact");
    private float crossfadeDuration = 0.1f;
    private float duration = 1f;
    public EnemyImpactState(EnemyStateMachine enemystateMachine) : base(enemystateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(impactHash, crossfadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        duration -= deltaTime;
        if(duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}
