using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponDamage : MonoBehaviourPunCallbacks
{
    private int attackDamage;
    private float knockBack;
    [SerializeField] private Collider myCollider;
    private List<Collider> alreadyCollidedwith = new List<Collider>();

    private void OnEnable() 
    {
        alreadyCollidedwith.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == myCollider) { return;}
        if(alreadyCollidedwith.Contains(other))
        { return;}
        alreadyCollidedwith.Add(other);
        
        if(other.TryGetComponent<PhotonView>(out PhotonView view))
        if(!view.IsMine)
        {
            if(other.TryGetComponent<Health>(out Health health) )
            {
                other.GetComponent<PhotonView>().RPC("DealDamage",RpcTarget.AllBuffered,attackDamage, myCollider.transform.position);
            }
            if(other.TryGetComponent<ForceReciever>(out ForceReciever forceReciever))
            {
                Vector3 forceDirection = (other.transform.position - myCollider.transform.position).normalized;
                other.GetComponent<PhotonView>().RPC("AddForce",RpcTarget.AllBuffered,forceDirection * knockBack);
            }
        }
    }
    public void SetAttackDamage(int damage, float knockBack)
    {
        attackDamage = damage;
        this.knockBack = knockBack;
    }
}
