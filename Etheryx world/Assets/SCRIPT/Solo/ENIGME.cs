using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ENIGME : MonoBehaviour
{
    public Coin coinScript;
    public GameObject MENU_ENIGME;
    public TMP_InputField inputField; // Utiliser TMP_InputField pour TextMesh Pro
    public string correctAnswer; // La bonne réponse que le joueur doit écrire
    public string enigmeID; // Identifiant unique pour chaque énigme

    private bool isrange;
    private bool MenuACT = false;
    public GameObject player; // Référence au joueur pour désactiver son mouvement

    public TextMeshProUGUI successText; // Référence au texte TMP à afficher
    private bool hasAnsweredCorrectly;

    void Start()
    {
        MENU_ENIGME.SetActive(false);

        // Charger l'état de la réponse depuis PlayerPrefs
        hasAnsweredCorrectly = PlayerPrefs.GetInt(enigmeID, 0) == 1;

        if (hasAnsweredCorrectly)
        {
            inputField.gameObject.SetActive(false);
            successText.gameObject.SetActive(true); // Afficher le texte TMP si déjà répondu correctement
        }
        else
        {
            successText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isrange && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuACT = !MenuACT;
            MENU_ENIGME.SetActive(MenuACT);

            if (MenuACT)
            {
                // Désactiver le script de mouvement du joueur
                player.GetComponent<perso_principal>().enabled = false;
                if (!hasAnsweredCorrectly)
                {
                    inputField.gameObject.SetActive(true); // Afficher la zone de texte
                    inputField.text = ""; // Réinitialiser le texte de l'InputField
                    inputField.ActivateInputField(); // Activer le champ d'entrée pour recevoir l'entrée de l'utilisateur
                }
            }
            else
            {
                // Réactiver le script de mouvement du joueur
                player.GetComponent<perso_principal>().enabled = true;
                inputField.gameObject.SetActive(false); // Masquer la zone de texte
            }
        }
    }

    public void CheckInput()
    {
        if (inputField.text == correctAnswer)
        {
            inputField.gameObject.SetActive(false); // Masquer la zone de texte
            hasAnsweredCorrectly = true; // Mettre à jour l'état pour indiquer que la bonne réponse a été donnée
            PlayerPrefs.SetInt(enigmeID, 1); // Sauvegarder l'état dans PlayerPrefs
            PlayerPrefs.Save();

            successText.gameObject.SetActive(true); // Afficher le texte TMP lorsque la réponse est correcte
            
            coinScript.IncrementCoinCount();
        }
    }

    public void ResetEnigme()
    {
        hasAnsweredCorrectly = false;
        PlayerPrefs.SetInt(enigmeID, 0); // Réinitialiser l'état dans PlayerPrefs
        PlayerPrefs.Save();
        inputField.gameObject.SetActive(true); // Réafficher la zone de texte
        successText.gameObject.SetActive(false); // Masquer le texte TMP
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isrange = true;
            Debug.Log("Player entered range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isrange = false;
            Debug.Log("Player exited range");
        }
    }
}