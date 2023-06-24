using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;
    private Attack attack;
    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackIndex) : base(playerStateMachine)
    {
        
        attack = stateMachine.attacks[attackIndex];
        
    }

    public override void Enter()
    {
        
        stateMachine.weaponDamage.SetAttackDamage(attack.Damage, attack.KnockBack);
        stateMachine.animator.CrossFade(attack.AnimationName, attack.TransitionDuration);
        
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);
        FaceTarget(deltaTime);

        float normalizedTime = GetNormalizedTime(stateMachine.animator, "Attack");
        
        if( normalizedTime < 1f)
        {
            if(normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }
            if(stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
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
        
    }

    

    

    private void TryComboAttack(float normalizedTime)
    {
        if( attack.ComboStateIndex == -1 ){ return;}

        if(normalizedTime <attack.ComboAttackTime) { return;}

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex
            )
        );
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce)
        {
            return;
        }
        alreadyAppliedForce = true;

        stateMachine.forceReciever.AddForce(stateMachine.transform.forward * attack.Force);
    }
}
