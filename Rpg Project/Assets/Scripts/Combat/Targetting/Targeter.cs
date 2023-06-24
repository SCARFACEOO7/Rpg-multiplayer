using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using Photon.Pun;

public class Targeter : MonoBehaviourPunCallbacks
{
    [SerializeField] Camera mainCamera;
    [SerializeField] private CinemachineTargetGroup cineTargets;
    private List<Target> targets = new List<Target>();
    public Target currentTarget {get; private set;}
    [SerializeField] PhotonView myView;

    private void Start() 
    {

    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<Target>(out Target newTarget))
        {
            if(newTarget.TryGetComponent<PhotonView>(out PhotonView view))
            if(view.IsMine)
            {
                return;
            }
            targets.Add(newTarget);
            newTarget.OnDestroyed += RemoveTarget;
        }
    }

    private void RemoveTarget(Target target)
    {
        if(currentTarget == target)
        {
            cineTargets.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    private void OnTriggerExit(Collider other) 
    {
        if(!other.TryGetComponent<Target>(out Target newTarget))
        {
            return;
        }

        RemoveTarget(newTarget);

        
    }

    public bool SelectTarget()
    {
        if(!myView.IsMine) { return false;}
        if(targets.Count == 0) {   return false;}
        if(mainCamera == null) {   return false;}

        Target closestTarget = null;
        float distancetoScreen = Mathf.Infinity;

        foreach(Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if(!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f,0.5f);
            if(toCenter.sqrMagnitude < distancetoScreen)
            {
                closestTarget = target;
                distancetoScreen = toCenter.sqrMagnitude;
            }
        }

        if(closestTarget == null) { return false;}

        currentTarget = closestTarget;
        cineTargets.AddMember(currentTarget.transform, 1f, 2f);

        return true;

    }

    public void Cancel()
    {
        if(currentTarget == null) { return;}
        cineTargets.RemoveMember(currentTarget.transform);
        currentTarget = null;
    }
}
