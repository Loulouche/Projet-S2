using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField] private string VersionName = "0.1";
    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private TMP_InputField UsernameInput;
    [SerializeField] private TMP_InputField CreateGameInput;
    [SerializeField] private TMP_InputField JoinGameInput;
    [SerializeField] private GameObject StartButton;

    private void Start()
    {
        UsernameMenu.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Already connected. Joining random room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Connecting to Photon...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = VersionName;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void ChangeUserNameInput()
    {
        StartButton.SetActive(UsernameInput.text.Length >= 3);
    }

    public void SetUserName()
    {
        UsernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.text;
        Debug.Log("Username set to: " + PhotonNetwork.NickName);
    }

    public void CreateGame()
    {
        Debug.Log("Creating room: " + CreateGameInput.text);
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = 5 }, null);
    }

    public void JoinGame()
    {
        Debug.Log("Joining room: " + JoinGameInput.text);
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 5 };
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room successfully");
        PhotonNetwork.LoadLevel("MULTI"); // Assurez-vous que la scène 1 est configurée dans les paramètres de build de Unity
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Create Room Failed: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join Room Failed: {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join Random Failed: {message}");
    }
}
