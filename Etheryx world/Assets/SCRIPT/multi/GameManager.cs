using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Assigne la préfabriqué du joueur dans l'inspecteur de Unity
    public GameObject sceneCamera; // Assigne la caméra de scène dans l'inspecteur de Unity

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in GameManager.");
            return;
        }

        float randomValue = Random.Range(-1f, 1f);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomValue, 0, 0), Quaternion.identity);
        
        if (sceneCamera != null) 
        {
            sceneCamera.SetActive(false); // Désactive la caméra de scène si elle est assignée
        }
        else
        {
            Debug.LogWarning("Scene camera is not assigned in GameManager.");
        }
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