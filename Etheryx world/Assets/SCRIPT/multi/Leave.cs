using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Leave : MonoBehaviourPunCallbacks
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetGameState(); // Réinitialiser l'état du jeu
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MENU");
    }

    void ResetGameState()
    {
        // Réinitialiser tous les états nécessaires ici
        PlayerPrefs.DeleteAll(); // Effacer les PlayerPrefs (ou réinitialiser seulement ceux nécessaires)
    }
}