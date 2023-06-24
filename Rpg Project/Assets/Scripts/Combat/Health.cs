using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks
{
    [SerializeField]private int maxHealth = 200;
    private int currentHealth;

    private bool isInvunerable = false;
    private bool isDodging = false;
    public event Action onTakeDamage;
    [SerializeField] PlayerStateMachine stateMachine;

    public bool IsDead => currentHealth==0;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void SetInvunerable(bool isInvunerable)
    {
        this.isInvunerable = isInvunerable;
    }
    public void SetDogeInvunerable(bool isDodging)
    {
        this.isDodging = isInvunerable;
    }

    [PunRPC]
    public void DealDamage(int damage, Vector3 position)
    {
        if(currentHealth <= 0) 
        { 
            return;
        }

        if(isInvunerable)
        {
            Vector3 direction = position - transform.position;
            if(Vector3.Angle(transform.forward, direction)<30)
            {
                return;
            }
        }

        if(isDodging)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        onTakeDamage.Invoke();
        if(currentHealth <= 0) 
        { 
            Die();
        }   
        Debug.Log(currentHealth);
    }

    private void Die()
    {
        if(photonView.IsMine)
        {
            stateMachine.Ragdoll.ToggleRagdoll(true);
            stateMachine.weaponDamage.gameObject.SetActive(false);
            stateMachine.animator.SetBool("IsDead", true);
            stateMachine.enabled = false;
        }
    }
}
