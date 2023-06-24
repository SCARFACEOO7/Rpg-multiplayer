using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("PullUp");
    private float CrossFadeDuration;
    private readonly Vector3 offset = new Vector3(0f, 2.325f, 0.65f);
    public PlayerPullUpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }

    public override void Enter()
    {
        
        stateMachine.animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
        
    }

    

    public override void Tick(float deltaTime)
    {
       
        if(GetNormalizedTime(stateMachine.animator, "Climbing") < 1f) { return;}
        stateMachine.characterController.enabled = false;
        stateMachine.transform.Translate(offset, Space.Self);
        stateMachine.characterController.enabled = true;
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
        
    }

    public override void Exit()
    {
        
        stateMachine.characterController.Move(Vector3.zero);
        stateMachine.forceReciever.Reset();
        
    }
}
