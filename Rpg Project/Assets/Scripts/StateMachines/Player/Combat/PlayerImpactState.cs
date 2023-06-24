using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int impactHash = Animator.StringToHash("Player Impact");
    private float crossfadeDuration = 0.1f;
    private float duration = 1f;
    public PlayerImpactState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
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
            ReturnToLocomotion();
        }
        
    }

    public override void Exit()
    {
    }

}
