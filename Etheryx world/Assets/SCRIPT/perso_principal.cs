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




    
    
    void Update()
    { 
        mouvement.x = Input.GetAxisRaw("Horizontal"); 
        mouvement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
        animator.SetFloat("Speed", mouvement.magnitude);
        
        rb.MovePosition(rb.position + mouvement * speed * Time.deltaTime);
          
    }

    




}