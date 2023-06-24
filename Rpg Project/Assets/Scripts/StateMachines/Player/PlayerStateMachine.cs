using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField]public Health health {get; private set;}
    [field: SerializeField]public InputReader InputReader;
    [field: SerializeField]public CharacterController characterController {get; private set;}
    [field: SerializeField]public Animator animator {get; private set;}
    [field: SerializeField]public Targeter targeter {get; private set;}
    [field: SerializeField]public ForceReciever forceReciever {get; private set;}
    [field: SerializeField]public Attack[] attacks {get; private set;}
    [field: SerializeField]public WeaponDamage weaponDamage {get; private set;}
    [field: SerializeField]public LedgeDetector LedgeDetector {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField]public float freelookMoveSpeed {get; private set;}
    [field: SerializeField]public float targettingMoveSpeed {get; private set;}
    [field: SerializeField]public float hangingMoveSpeed {get; private set;}
    [field: SerializeField]public float freeLookRotationDamping {get; private set;}
    [field: SerializeField]public float targettingRotationDamping {get; private set;}
    [field: SerializeField]public float DodgeDuration {get; private set;}
    [field: SerializeField]public float DodgeLength {get; private set;}
    [field: SerializeField]public float PrevoiusDodgeTime {get; private set;} = Mathf.NegativeInfinity;
    [field: SerializeField]public float DodgeCoolDown {get; private set;}
    [field: SerializeField]public float JumpForce {get; private set;}
    [field: SerializeField]public Camera mainCamera;
    
    
    public Transform mainCameraTransform {get; private set;}
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        mainCameraTransform = mainCamera.transform;
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
        SwitchState(new PlayerImpactState(this));
    }




    public void SetDodgeTime(float time)
    {
        PrevoiusDodgeTime = time;
    }
    
}
