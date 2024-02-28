using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class perso_principal : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 mouvement;
    public Animator animator;



    private bool modif = false;
    private void Start()
    {
        
        
        transform.position = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));
       

        
    }
    void Update()
    { 
    
        mouvement.x = Input.GetAxisRaw("Horizontal"); 
        mouvement.y = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);


        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);

        rb.MovePosition(rb.position + mouvement * speed * Time.deltaTime);

    }








}