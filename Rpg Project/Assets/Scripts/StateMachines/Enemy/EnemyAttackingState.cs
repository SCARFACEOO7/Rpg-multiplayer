using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private const float transitionDuration = 0.1f;
    public EnemyAttackingState(EnemyStateMachine enemystateMachine) : base(enemystateMachine)
    {
    }

    public override void Enter()
    {
        FacePlayer();
        stateMachine.animator.CrossFadeInFixedTime(AttackHash, transitionDuration);
        stateMachine.weapon.SetAttackDamage(stateMachine.AttackDamage, stateMachine.AttackKnockback);
    }
    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.animator, "Attack") >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }

    
}
