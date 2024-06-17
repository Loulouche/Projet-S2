using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Coin : MonoBehaviour
{ 
    public Restart ScriptRestart;
    public GameObject ANIM;
    public TextMeshProUGUI coinText; // Référence au texte affichant le nombre de pièces
    private int coinCount = 0; // Nombre de pièces collectées

    
    
    void Start()
    {
        ANIM.SetActive(false);
        // Vérifier si le joueur a déjà collecté des pièces dans une autre scène
        if (PlayerPrefs.HasKey("CoinCount"))
        {
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }
        UpdateCoinText(); // Mettre à jour le texte des pièces
    }

    public void IncrementCoinCount()
    {
        coinCount++; // Incrémenter le nombre de pièces
        UpdateCoinText(); // Mettre à jour le texte des pièces

        
        // Vérifier si le joueur a collecté suffisamment de pièces pour la transition vers une autre scène
      
    }

    
    public void ResetCoinCount()
    {
        coinCount = 0; // Réinitialiser le nombre de pièces
        UpdateCoinText(); // Mettre à jour le texte des pièces
    }

    void Update()
    {
        if (coinCount >= 1)
        {
            if (ANIM != null)
            {
                ANIM.SetActive(true);
            }
            if (ScriptRestart != null)
            {
                ScriptRestart.Recommencer();
            }
           
        }
    }
    void UpdateCoinText()
    {
        coinText.text = "" + coinCount; // Mettre à jour le texte des pièces
       
    }

    void OnDestroy()
    {
        // Sauvegarder le nombre de pièces collectées lorsque le script est détruit (changement de scène)
        PlayerPrefs.SetInt("CoinCount", coinCount);
        PlayerPrefs.Save();
    }

}
