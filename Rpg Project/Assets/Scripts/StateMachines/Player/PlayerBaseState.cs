using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
    }
    protected void FaceTarget(float deltaTime)
    {
        if(stateMachine.targeter.currentTarget == null) { return;}

        Vector3 lookPos = stateMachine.targeter.currentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
         Quaternion.LookRotation(lookPos), stateMachine.targettingRotationDamping * deltaTime);

    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.characterController.Move((motion + stateMachine.forceReciever.movement) * deltaTime);
    }

    protected void ReturnToLocomotion()
    {
        if(stateMachine.targeter.currentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpinState(stateMachine));
    }

    
    
}
