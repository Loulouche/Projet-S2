using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject Gamemanager;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool isrange;
    private bool isDialogueStarted;

    void Start()
    {
        textComponent.text = string.Empty;
        Gamemanager.SetActive(false);
    }

    void Update()
    {
        if (isrange && !isDialogueStarted)
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
        {textComponent.text = string.Empty;
            isDialogueStarted = false;
            Debug.Log("Dialogue ended");
        }
    }

    void EndDialogue()
    {
        Gamemanager.SetActive(false);
        textComponent.text = string.Empty;
        isDialogueStarted = false;
        Debug.Log("Dialogue ended");
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
            if (isDialogueStarted)
            {
                EndDialogue();
            }
            Debug.Log("Player exited range");
        }
    }
}