using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Health health {get; private set;}
    [field: SerializeField] public Animator animator {get; private set;}
    [field: SerializeField] public Health player {get; private set;}
    [field: SerializeField] public CharacterController characterController {get; private set;}
    [field: SerializeField] public ForceReciever forceReciever {get; private set;}
    [field: SerializeField] public NavMeshAgent agent {get; private set;}
    [field: SerializeField] public WeaponDamage weapon {get; private set;}
    [field: SerializeField] public Target Target {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField] public int AttackDamage {get; private set;}
    [field: SerializeField] public float MovementSpeed {get; private set;}
    [field: SerializeField] public float PlayerChasingRange {get; private set;}
    [field: SerializeField] public float AttackRange {get; private set;}
    [field: SerializeField] public float AttackKnockback {get; private set;}
    
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }

    private void OnEnable() 
    {
        health.onTakeDamage +=  HandleTakeDamage;
    }

    private void OnDisable() 
    {
        health.onTakeDamage -=  HandleTakeDamage;
    }
    
    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDeath()
    {
        SwitchState(new EnemyDeadState (this));
    }
}
