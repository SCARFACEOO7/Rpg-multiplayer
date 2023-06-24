using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpinState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;
    public PlayerJumpinState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        
        stateMachine.forceReciever.Jump(stateMachine.JumpForce);
        momentum = stateMachine.characterController.velocity;
        momentum.y = 0f;
        stateMachine.animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    
    }
    public override void Tick(float deltaTime)
    {
        
        Move(momentum, deltaTime);
        FaceTarget(deltaTime);
        if(stateMachine.characterController.velocity.y <= 0f)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }
        
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
