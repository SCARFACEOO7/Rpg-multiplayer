using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Ragdoll : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    private Collider[] colliders;
    private Rigidbody[] rigidbodies;
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>(true);
        rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        if(!this.photonView.IsMine)
        {
            return;
        }
        foreach(Collider collider in colliders)
        {
            if(collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            if(rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }

}
