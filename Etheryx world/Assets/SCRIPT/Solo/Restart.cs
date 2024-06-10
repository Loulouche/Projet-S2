using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour
{
    public Coin coinScript;
    
    public Button restartButton; // Référence au bouton de redémarrage
    public GameObject player; // Référence au joueur
    public List<ENIGME> enigmes; // Liste de toutes les énigmes à réinitialiser

    
    private List<string> enigmeIDs = new List<string> { "enigme1" }; // Ajouter les IDs de toutes les énigmes
    
    void Start()
    {
        // Ajouter l'écouteur d'événements au bouton
        restartButton.onClick.AddListener(Recommencer);
    }

    void Recommencer()
    {
        // Réinitialiser la position du joueur
        player.transform.position = Vector3.zero;
    
        // Réinitialiser les états des énigmes
        foreach (var enigmeID in enigmeIDs)
        {
            PlayerPrefs.SetInt(enigmeID, 0); // Réinitialiser l'état dans PlayerPrefs
        }

        // Réinitialiser le nombre de pièces
        coinScript.ResetCoinCount();

        // Optionnel: Réinitialiser les PlayerPrefs si nécessaire
        PlayerPrefs.DeleteKey("HasAnsweredCorrectly");
        PlayerPrefs.Save();
    }
}

