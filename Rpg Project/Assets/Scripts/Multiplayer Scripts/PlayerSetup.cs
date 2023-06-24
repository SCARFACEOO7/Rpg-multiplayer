using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    private InputReader inputReader;
    private PlayerStateMachine stateMachine;
    public Camera mainCamera;
    public GameObject[] cameras;
    void Start()
    {
        inputReader = GetComponent<InputReader>();
        stateMachine = GetComponent<PlayerStateMachine>();
        if (this.photonView.IsMine)
        {
            stateMachine.InputReader = inputReader;
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            mainCamera.enabled = true;
        }
        else
        {
            mainCamera.gameObject.GetComponent<CinemachineBrain>().enabled = false;
            mainCamera.enabled = false;
            foreach(GameObject camera in cameras)
            {
                Destroy(camera);
            }
        }

    }


}
