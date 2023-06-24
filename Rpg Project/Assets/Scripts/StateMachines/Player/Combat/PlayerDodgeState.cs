using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private float remainingDodgeTime;
    private const float CrossFadeDuration = 0.1f;
    private readonly int DodgetreeHash = Animator.StringToHash("Dodge Blend Tree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    Vector2 dodgeDirectionInput;

    public PlayerDodgeState(PlayerStateMachine playerStateMachine, Vector2 dodgingDirection) : base(playerStateMachine)
    {
        
        dodgeDirectionInput = dodgingDirection;
        
    }

    public override void Enter()
    {
        
        remainingDodgeTime = stateMachine.DodgeDuration;
        stateMachine.animator.SetFloat(DodgeForwardHash, dodgeDirectionInput.y);
        stateMachine.animator.SetFloat(DodgeRightHash, dodgeDirectionInput.x);
        stateMachine.animator.CrossFadeInFixedTime(DodgetreeHash, CrossFadeDuration);
        stateMachine.health.SetDogeInvunerable(true);
        
    }

    public override void Exit()
    {
        
        stateMachine.health.SetDogeInvunerable(false);
        
    }

    public override void Tick(float deltaTime)
    {
        
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgeDirectionInput.x * stateMachine.DodgeLength/stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgeDirectionInput.y * stateMachine.DodgeLength/stateMachine.DodgeDuration;
        Move(movement * stateMachine.targettingMoveSpeed, deltaTime);   
        FaceTarget(deltaTime);
        remainingDodgeTime = Mathf.Max(remainingDodgeTime - deltaTime, 0f);
        if(remainingDodgeTime <= 0f)
        {
            ReturnToLocomotion();
        }
    }
    


    
}
