using UnityEngine;
using Photon.Pun; // Nécessaire pour PhotonNetwork
using Photon.Realtime; // Nécessaire pour les callbacks de Photon
using UnityEngine.SceneManagement;

public class Leave : MonoBehaviourPunCallbacks
{
   void Update()
       {
           // Vérifie si la touche "Echap" est pressée
           if (Input.GetKeyDown(KeyCode.Escape))
           {
               // Quitte la room Photon
               PhotonNetwork.LeaveRoom();
           }
       }
   
       // Callback appelée lorsque le joueur a quitté la room
       public override void OnLeftRoom()
       {
           // Charge la scène "Main Menu"
           SceneManager.LoadScene("MENU");
       }
}
