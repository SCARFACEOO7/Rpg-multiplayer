using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class ForceReciever : MonoBehaviourPunCallbacks
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private CharacterController controller;
    [SerializeField] float drag = 0.3f;
    private float verticalVelocity;
    private Vector3 dampVelocity;
    private Vector3 impact;
    public Vector3 movement => impact + (Vector3.up * verticalVelocity);
    
    private void Update() 
    {
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampVelocity, drag);
        if(agent != null)
        {
            if(impact.sqrMagnitude <= 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }
        
    }

    [PunRPC]
    public void AddForce(Vector3 force)
    {
        impact += force;
        if(agent != null)
        {
            agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        verticalVelocity = 0f;
        impact = Vector3.zero;    
    }
}
