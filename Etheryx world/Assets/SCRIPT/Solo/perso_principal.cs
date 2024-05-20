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
	public float vie;
	public float maxvie;

    [Header("Attack")]
    private float attacktime;

    [SerializeField] 
    private float timeBetweenattack;

    private bool canMove;
    [SerializeField] private Transform checkEnemy;
    public LayerMask whatIsEnemy;

    public float range;
    
	public static perso_principal instance;

	private void Awake()
	{
		instance = this;
	}

    private void Start()
    {
        vie = maxvie;
        transform.position = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
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
                rb.velocity = Vector2.zero;
                anim.SetTrigger("attack");

                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    canMove = false;
                    yield return new WaitForSeconds(.5f);
                    canMove = true;
                }
                attacktime = Time.time + timeBetweenattack;
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (canMove)
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

        if (Input.GetAxis("Horizontal") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x + range, transform.position.y, 0);
        }
        else if (Input.GetAxis("Horizontal") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x - range, transform.position.y, 0);
        }
        if (Input.GetAxis("Vertical") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y +range, 0);
        }
        else if (Input.GetAxis("Vertical") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y -range, 0);
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;

        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        rb.velocity.Normalize();

        if (x != 0 || y != 0)
        {
            anim.SetFloat("H", x);
            anim.SetFloat("V", y);
        }
    }


    public void OnAttack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(checkEnemy.position, 0.5f, whatIsEnemy);

        foreach (var enemy_ in enemy)
        {
            //degats
        }
    }
    
    public void TakeDamage( float damage)
    {
        {
            vie -= damage;
        }
    }
}