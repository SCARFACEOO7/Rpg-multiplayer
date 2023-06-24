using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Block");
    private float blockTransitionDuration = 0.1f;
    public PlayerBlockingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        
        stateMachine.health.SetInvunerable(true);
        stateMachine.animator.CrossFadeInFixedTime(BlockHash, blockTransitionDuration);
        
    }
    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);
        if(stateMachine.InputReader==null)
        {
            return;
        }
        if(!stateMachine.InputReader.IsBlocking)
        {
            if(stateMachine.targeter.currentTarget == null)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
        }
        
    }

    public override void Exit()
    {
        
        stateMachine.health.SetInvunerable(false);
        
    }

    
}
