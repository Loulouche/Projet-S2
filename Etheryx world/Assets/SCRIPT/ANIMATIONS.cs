
   using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ANIMATIONS : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    
    public GameObject Gamemanager;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool isDialogueStarted;

    void Start()
    {
        textComponent.text = string.Empty;
        Gamemanager.SetActive(false);
        // Optionnel : démarrer le dialogue dès le début
        StartDialogue();
    }

    void Update()
    {
        // Commence le dialogue lorsque la touche "D" est enfoncée
        if (!isDialogueStarted && Input.GetKeyDown(KeyCode.Return))
        {
            Gamemanager.SetActive(true);
            StartDialogue();
            isDialogueStarted = true;
            Debug.Log("Dialogue started");
        }

        if (isDialogueStarted && Input.GetKeyDown(KeyCode.Return))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    
    void EndDialogue()
    {
        Gamemanager.SetActive(false);
        textComponent.text = string.Empty;
        isDialogueStarted = false;
        Debug.Log("Dialogue ended");
        SceneManager.LoadScene("MENU");
    }

   
    
    
}

