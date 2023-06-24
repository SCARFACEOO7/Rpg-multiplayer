using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    

    #region Serializable field
    public TMP_Text connectionStatus;

    [Header("Login Panel")]
    public GameObject loginUIPanel;
    public TMP_InputField playerNametext;

    [Header("Game Options Panel")]

    public GameObject gameOptionsPanel;

    [Header("Create Room Panel")]

    public GameObject hostGamePanel;
    public TMP_InputField roomNameField;
    public TMP_InputField maxPlayersField;
    
    [Header("Inside Room Panel")]
    public GameObject insideRoomPanel;
    public TMP_Text roomInfoText;
    public GameObject startButton;

    #endregion


    #region unitymethods

    private void Awake() 
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start() 
    {
        ActivatePanel(loginUIPanel.name);
    }
    private void Update() 
    {
        connectionStatus.text = "Connection Status:" + PhotonNetwork.NetworkClientState;
    }
    #endregion


    #region UICallback

    public void OnLoginButtonClicked()
    {
        string playerName = playerNametext.text;
        if(!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnHostButtonClicked()
    {
        ActivatePanel(hostGamePanel.name);
    }

    public void OnJoinButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameField.text;
        if(string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1000,10000);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)int.Parse(maxPlayersField.text);
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartButtonClicked()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Sandbox_New");
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }
    #endregion


    #region PUNCallback
    public override void OnConnectedToMaster()
    {
        ActivatePanel(gameOptionsPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ActivatePanel(hostGamePanel.name);
    }

    public override void OnJoinedRoom()
    {
        ActivatePanel(insideRoomPanel.name);
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }

        roomInfoText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name + 
        "  " + PhotonNetwork.CurrentRoom.PlayerCount + 
        "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(gameOptionsPanel.name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomInfoText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name + 
        "  " + PhotonNetwork.CurrentRoom.PlayerCount + 
        "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        roomInfoText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name + 
        "  " + PhotonNetwork.CurrentRoom.PlayerCount + 
        "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }

    #endregion

    #region private methods
    private void ActivatePanel(string panelToActivate)
    {
        loginUIPanel.SetActive(panelToActivate.Equals(loginUIPanel.name));
        gameOptionsPanel.SetActive(panelToActivate.Equals(gameOptionsPanel.name));
        hostGamePanel.SetActive(panelToActivate.Equals(hostGamePanel.name));
        insideRoomPanel.SetActive(panelToActivate.Equals(insideRoomPanel.name));
    }

    #endregion
    
}
