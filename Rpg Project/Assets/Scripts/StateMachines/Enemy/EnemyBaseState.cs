using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;
    
    public EnemyBaseState(EnemyStateMachine enemystateMachine)
    {
        stateMachine = enemystateMachine;
    }

    protected bool IsInChaseRange()
    {
        if(stateMachine.player.IsDead) {return false; }
        float playerdistanceSquare = (stateMachine.player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return (playerdistanceSquare <= stateMachine.PlayerChasingRange*stateMachine.PlayerChasingRange);
    }

    protected bool IsInAttackRange()
    {
        if(stateMachine.player.IsDead) {return false; }
        float playerdistancesquare = (stateMachine.player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return (playerdistancesquare <= stateMachine.AttackRange*stateMachine.AttackRange);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.characterController.Move((motion + stateMachine.forceReciever.movement) * deltaTime);
    }

    

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FacePlayer()
    {
        if(stateMachine.player == null) { return;}

        Vector3 lookPos = stateMachine.player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

}
