
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject sceneCamera;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f, 1f);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomValue, 0, 0), Quaternion.identity);
        gameCanvas.SetActive(false);
        sceneCamera.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        SpawnPlayer();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }
}
