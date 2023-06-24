using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPGGameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        { 
            Vector3 spawpoint = new Vector3(Random.Range(-50, -35), 12,Random.Range(10,20));
            PhotonNetwork.Instantiate(playerPrefab.name,spawpoint,Quaternion.identity);
        }
    }

   
}
