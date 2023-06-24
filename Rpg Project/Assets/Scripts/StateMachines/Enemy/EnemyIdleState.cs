using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
        private readonly int locomotionTreeHash = Animator.StringToHash("Locomotion");
        private readonly int speedhash = Animator.StringToHash("Speed");
        private const float crossFadeDuration = 0.1f;
        private const float animatorDampTime = 0.1f;
    public EnemyIdleState(EnemyStateMachine enemystateMachine) : base(enemystateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(locomotionTreeHash, crossFadeDuration);
        
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        stateMachine.animator.SetFloat(speedhash, 0f, animatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }

    
}
