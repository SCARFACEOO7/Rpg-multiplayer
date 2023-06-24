using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");
    private Vector3 momentum;
    private float animatorDampTime = 0.1f;
    public PlayerFallingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        
        momentum = stateMachine.characterController.velocity;
        momentum.y = 0f;
        stateMachine.animator.CrossFadeInFixedTime(FallHash, animatorDampTime);
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        
    }
    public override void Tick(float deltaTime)
    {
        
        Move(momentum, deltaTime);
        if(stateMachine.characterController.isGrounded)
        {
            ReturnToLocomotion();
            return;
        }
        FaceTarget(deltaTime);
        
    }

    public override void Exit()
    {
        
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
        
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }
}
