using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenImage : MonoBehaviour
{
    public GameObject imageObject; // L'image à afficher
    private bool isPlayerInRange = false; // Détecte si le joueur est dans la zone de détection
    private bool isImageOpen = false; // Détecte si l'image est ouverte

    void Start()
    {
        if (imageObject != null)
        {
            imageObject.SetActive(false); // Assurez-vous que l'image est désactivée au départ
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isImageOpen)
            {
                CloseImage();
            }
            else
            {
                OpenImageFunction();
            }
        }
    }

    void OpenImageFunction()
    {
        if (imageObject != null)
        {
            imageObject.SetActive(true); // Afficher l'image
            Time.timeScale = 0f; // Arrêter le temps
            isImageOpen = true;
        }
    }

    void CloseImage()
    {
        if (imageObject != null)
        {
            imageObject.SetActive(false); // Masquer l'image
            Time.timeScale = 1f; // Reprendre le temps
            isImageOpen = false;
        }
    }

  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player exited range");
        }
    }
}