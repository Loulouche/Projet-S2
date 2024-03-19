using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class perso_principal : MonoBehaviour
{

    [Header("Component")] 
    private Rigidbody2D rb;

    private Animator anim;

    [Header("Stat")] [SerializeField] 
    private float moveSpeed;

    [Header("Attack")]
    private float attacktime;

    [SerializeField] 
    private float timeBetweenattack;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= attacktime)
            {
                anim.SetTrigger("attack");

                attacktime = Time.time + timeBetweenattack;
            }
        }
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
        if (Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 ||
            Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
        {
            anim.SetFloat("x", Input.GetAxis("Horizontal"));
            anim.SetFloat("y", Input.GetAxis("Vertical"));
        }
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;

        rb.velocity.Normalize();

        if (x != 0 || y != 0)
        {
            anim.SetFloat("H", x);
            anim.SetFloat("V", y);
        }
    }

}