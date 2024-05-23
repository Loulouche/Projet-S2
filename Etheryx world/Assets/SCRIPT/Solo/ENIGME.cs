using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ENIGME : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public GameObject MENU_ENIGME;
    private bool isrange;
    public GameObject POINT;
    private bool MenuACT = false;

    // Update is called once per frame
    void Update()
    {
        if (isrange && Input.GetKeyDown(KeyCode.E) && !MenuACT)
        {
            MenuACT = true;
            MENU_ENIGME.SetActive(true);
          
        }
        if (isrange && Input.GetKeyDown(KeyCode.E) && MenuACT)
        {
            MenuACT = false;
            MENU_ENIGME.SetActive(false);
            
        }
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
