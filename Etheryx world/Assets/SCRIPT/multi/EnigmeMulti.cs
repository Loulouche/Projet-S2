using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class EnigmeMulti : MonoBehaviourPunCallbacks
{

    public GameObject MENUDISCUSSION;
    public GameObject MENU_ENIGME;
    public TMP_InputField inputField;
    public string correctAnswer;
    public string enigmeID;

    private bool isRange;
    private bool MenuACT = false;
    private GameObject localPlayer;

    private bool hasAnsweredCorrectly;

    public GameObject successImage; // Image à afficher en cas de succès
    public GameObject failureImage; // Image à afficher en cas d'échec

    void Start()
    {
        MENU_ENIGME.SetActive(false);
        successImage.SetActive(false); // Désactiver l'image de succès au début
        failureImage.SetActive(false); // Désactiver l'image d'échec au début

        // Charger l'état de la réponse depuis PlayerPrefs
        hasAnsweredCorrectly = PlayerPrefs.GetInt(enigmeID, 0) == 1;

        if (hasAnsweredCorrectly)
        {
            inputField.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        if (isRange && Input.GetKeyDown(KeyCode.Space) && localPlayer != null && localPlayer.GetComponent<PhotonView>().IsMine)
        {
            successImage.SetActive(false); // Désactiver l'image de succès au début
            failureImage.SetActive(false); // Désactiver l'image d'échec au début
            
            inputField.gameObject.SetActive(true);
            
            MenuACT = !MenuACT;
            MENU_ENIGME.SetActive(MenuACT);

            if (MenuACT)
            {
                // Désactiver le script de mouvement du joueur
                localPlayer.GetComponent<Player>().enabled = false;
                if (!hasAnsweredCorrectly)
                {
                    inputField.gameObject.SetActive(true);
                    inputField.text = "";
                    inputField.ActivateInputField();
                }
            }
            else
            {
                // Réactiver le script de mouvement du joueur
                localPlayer.GetComponent<Player>().enabled = true;
            }
        }
    }

    public void OnSubmitButtonClick()
    {
        CheckInput(inputField.text);
    }

    public void CheckInput(string userInput)
    {
        Debug.Log("CheckInput called with input: " + userInput);
        if (userInput == correctAnswer)
        {
            Debug.Log("Correct answer entered");
            hasAnsweredCorrectly = true;

            // Sauvegarder l'état dans PlayerPrefs
            PlayerPrefs.SetInt(enigmeID, 1);
            PlayerPrefs.Save();


            // Afficher l'image de succès et masquer l'image d'échec
            successImage.SetActive(true);
            failureImage.SetActive(false);
        }
        else
        {
            Debug.Log("Incorrect answer entered");

            // Afficher l'image d'échec et masquer l'image de succès
            successImage.SetActive(false);
            failureImage.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PhotonView playerPhotonView = collision.GetComponent<PhotonView>();
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                isRange = true;
                localPlayer = collision.gameObject;
                Debug.Log("Player entered range");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PhotonView playerPhotonView = collision.GetComponent<PhotonView>();
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                isRange = false;
                MENU_ENIGME.SetActive(false);
                localPlayer.GetComponent<Player>().enabled = true;
                localPlayer = null;
                Debug.Log("Player exited range");
            }
        }
    }
}
