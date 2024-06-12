using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Restart : MonoBehaviour
{    

    
    public GameObject enemyPrefab;
    public Enemy enemy;
    public Transform enemySpawnPoint ; // Référence au point de spawn de l'ennemi

    
    public Coin coinScript;
    public perso_principal playerScript; // Référence au script du joueur

    public Button restartButton; // Référence au bouton de redémarrage
    public GameObject player; // Référence au joueur
    public List<string> enigmeIDs = new List<string> { "enigme1", "enigme2", "enigme3", "enigme4", "enigme5", "enigme6", "enigme7", "enigme8", "enigme9", "enigme10", "enigme11", "enigme12", "enigme13", "enigme14" }; // Ajouter les IDs de toutes les énigmes

    void Start()
    {
        // Ajouter l'écouteur d'événements au bouton
        restartButton.onClick.AddListener(Recommencer);
        
        enemySpawnPoint = GameObject.Find("Enemy").transform; // Remplacez "YourEnemySpawnPoint" par le nom de votre objet dans la scène

    }

    
    public void Recommencer()
    {
        // Réinitialiser la position du joueur
        player.transform.position = Vector3.zero;

        // Réinitialiser les états des énigmes dans PlayerPrefs
        foreach (var enigmeID in enigmeIDs)
        {
            PlayerPrefs.SetInt(enigmeID, 0);
        }

        // Réinitialiser le nombre de pièces
        coinScript.ResetCoinCount();

        // Réinitialiser la vie du joueur en utilisant le script du joueur
        playerScript.ResetPlayerLife();

        // Réinitialiser l'ennemi si l'ennemi est null, c'est pas bon
        if (enemy != null)
        {
            enemy.ResetEnemy(); // Appel de la méthode ResetEnemy() de l'ennemi
        }
        else
        {
            // Réinstancier l'ennemi s'il est null (cette logique dépend de comment l'ennemi est instancié dans votre jeu)
            // Exemple de réinstanciation si l'ennemi est détruit
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            enemy = newEnemy.GetComponent<Enemy>();
        }
    }

    

    public void Retry()
    {
        
        Time.timeScale = 1f;
        Recommencer();
    }
}