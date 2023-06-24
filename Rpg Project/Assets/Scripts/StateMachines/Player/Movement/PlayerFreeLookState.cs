using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendtreeHash = Animator.StringToHash("Free Look Blend Tree");
    private const float animatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    private bool shouldFade;
    public PlayerFreeLookState(PlayerStateMachine playerStateMachine, bool shouldFade = true) : base(playerStateMachine)
    {
        
        this.shouldFade = shouldFade;
        
    }

    public override void Enter()
    {
        
        if(stateMachine.InputReader!=null)
        {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;
        }
        stateMachine.animator.SetFloat(FreeLookSpeedHash, 0f);

        if(shouldFade)
        {
            stateMachine.animator.CrossFadeInFixedTime(FreeLookBlendtreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.animator.Play(FreeLookBlendtreeHash);  
        }
        
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
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.freelookMoveSpeed, deltaTime);
        if(stateMachine.InputReader.MoveValue == Vector2.zero) 
        {
            stateMachine.animator.SetFloat(FreeLookSpeedHash, 0f, animatorDampTime, deltaTime);
            return;
        }
        stateMachine.animator.SetFloat(FreeLookSpeedHash, 1f, animatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
        
    }
    public override void Exit()
    {
        
        if(stateMachine.InputReader!=null)
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.JumpEvent -= OnJump;
        }
        
    }

    

    private void OnTarget()
    {
        if(!stateMachine.targeter.SelectTarget()) { return; }
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.mainCameraTransform.forward;
        Vector3 right = stateMachine.mainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MoveValue.y + 
        right * stateMachine.InputReader.MoveValue.x;
    }

    void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.freeLookRotationDamping);
    }
   

    



    
}
