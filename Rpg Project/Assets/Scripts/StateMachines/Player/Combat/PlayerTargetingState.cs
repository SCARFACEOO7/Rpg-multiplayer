using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendtreeHash = Animator.StringToHash("Targeting Blend Tree");
    private readonly int TargetingForwardHash = Animator.StringToHash("ForwardSpeedTarget");
    private readonly int TargetingRightHash = Animator.StringToHash("RightTarget");
    private const float CrossFadeDuration = 0.1f;
    public PlayerTargetingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        
        stateMachine.animator.CrossFadeInFixedTime(TargetingBlendtreeHash, CrossFadeDuration);
        if(stateMachine.InputReader==null)
        {
            return;
        }
        stateMachine.InputReader.TargetEvent += OnTargetCancel;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.animator.CrossFadeInFixedTime(TargetingBlendtreeHash, CrossFadeDuration);
        
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.InputReader==null)
        {
            return;
        }
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        if(stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        
        if(stateMachine.targeter.currentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.targettingMoveSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget(deltaTime);
        


    }

    private void UpdateAnimator(float deltaTime)
    {
        if(stateMachine.InputReader==null)
        {
            return;
        }
        if(stateMachine.InputReader.MoveValue.y == 0f)
        {
            stateMachine.animator.SetFloat(TargetingForwardHash, 0f, 0.1f, deltaTime);
            
        }
        else
        {
            float value = stateMachine.InputReader.MoveValue.y > 0? 1f : -1f;
            stateMachine.animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }
        if(stateMachine.InputReader.MoveValue.x == 0f)
        {
            stateMachine.animator.SetFloat(TargetingRightHash, 0f, 0.1f, deltaTime);
            
        }
        else
        {
            float value = stateMachine.InputReader.MoveValue.x > 0? 1f : -1f;
            stateMachine.animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
        
    }

    public override void Exit()
    {
        
        if(stateMachine.InputReader==null)
        {
            return;
        }
        stateMachine.InputReader.TargetEvent -= OnTargetCancel;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
        
    }


    private void OnTargetCancel()
    {
        stateMachine.targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MoveValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MoveValue.y;

        return movement;
    }
    protected void OnDodge()
    {
        if(stateMachine.InputReader==null)
        {
            return;
        }
        if(stateMachine.InputReader.MoveValue == Vector2.zero) { return;}
        stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MoveValue));
    }

   


    
}
