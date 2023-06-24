using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int locomotionTreeHash = Animator.StringToHash("Locomotion");
    private readonly int speedhash = Animator.StringToHash("Speed");
    private const float crossFadeDuration = 0.1f;
    private const float animatorDampTime = 0.1f;
    public EnemyChasingState(EnemyStateMachine enemystateMachine) : base(enemystateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(locomotionTreeHash, crossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if(!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        if(IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        FacePlayer();
        stateMachine.animator.SetFloat(speedhash, 1f, animatorDampTime, deltaTime);
    }

    

    public override void Exit()
    {
        if(stateMachine.agent.isOnNavMesh)
        {
            stateMachine.agent.ResetPath();
        }
        
        stateMachine.agent.velocity = Vector3.zero;
    }
    private void MoveToPlayer(float deltaTime)
    {
        if(stateMachine.agent.isOnNavMesh)
        {
            stateMachine.agent.destination = stateMachine.player.transform.position;
            Move(stateMachine.agent.desiredVelocity.normalized * stateMachine.MovementSpeed,deltaTime);
        }
        stateMachine.agent.velocity = stateMachine.characterController.velocity;
    }

    
}
