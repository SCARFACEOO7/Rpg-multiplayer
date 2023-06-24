using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LedgeDetector : MonoBehaviour
{
    public Collider ledge;
    public event Action<Vector3,Vector3> OnLedgeDetect;
    private void OnTriggerEnter(Collider other) 
    {
        ledge = other;
        OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
    }

    private void OnTriggerExit(Collider other) 
    {
        ledge = null;
    }
}
