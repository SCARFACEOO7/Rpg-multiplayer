using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingBlendTreeHash = Animator.StringToHash("Hanging Blend Tree");
    private float CrossFadeDuration;
    private float pullUpDelay = 1f;
    private Vector3 ledgeForward;
    private Vector3 closestPoint;
    public PlayerHangingState(PlayerStateMachine playerStateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(playerStateMachine)
    {
        
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
        
    }

    public override void Enter()
    {
        
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        stateMachine.characterController.enabled = false;
        stateMachine.transform.position = closestPoint -(stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.characterController.enabled = true;
        stateMachine.animator.CrossFadeInFixedTime(HangingBlendTreeHash, CrossFadeDuration);
        
    }

    public override void Tick(float deltaTime)
    {
        
        stateMachine.animator.SetFloat("Hanging Input", stateMachine.InputReader.MoveValue.x);
        if(pullUpDelay == 0f)
        {
            return;
        }
        else
        {
            pullUpDelay = Mathf.Max(Mathf.Abs(pullUpDelay - deltaTime), 0f);
        }
        if(stateMachine.InputReader==null)
        {
            return;
        }
        if(stateMachine.InputReader.MoveValue.y > 0f)
        {
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }
        else if(stateMachine.InputReader.MoveValue.y < 0f || stateMachine.LedgeDetector.ledge == null)
        {
            stateMachine.characterController.Move(Vector3.zero);
            stateMachine.forceReciever.Reset();
            stateMachine.SwitchState(new PlayerJumpinState(stateMachine));
        }
        stateMachine.characterController.Move(CalCulateMovement() * deltaTime);
        
        
    }

    private Vector3 CalCulateMovement()
    {
        if(stateMachine.LedgeDetector.ledge == null) { return Vector3.zero;}
        Vector3 movement = new Vector3();
        movement += stateMachine.LedgeDetector.ledge.transform.right*
        stateMachine.InputReader.MoveValue.x*
        stateMachine.hangingMoveSpeed;

        return movement;

    }

    public override void Exit()
    {
        
    }

    
}
