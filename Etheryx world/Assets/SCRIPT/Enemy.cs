using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using Pathfinding;

public class Enemy : MonoBehaviour
{
    
    [Header("Stats")] 
    [SerializeField] float speed;
    private float playerDetectTime;
    public float playerDetectRate;
    public float chaseRange;
    bool lookRight;
    
    [Header("Attack")] 
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] float attaclRate;
    private float lastAttackTime;
    public Transform attackPoint;
    public LayerMask playerLayerMask;
    

    [Header("Component")] 
    Rigidbody2D rb;
    private perso_principal targetPlayer;
    Animator anim ;
    
    
    [Header("Pathfinding")] 
    public float nextWaypointDistance = 2f;
    Path path;
    int currentWayPoint = 0;
    bool reachEndPath = false;
    Seeker seeker;

    
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
		anim= GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .5f);
       
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetPlayer != null)
        {
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }
    
    private void Update()
    {
       if (rb.velocity.x < 0 && !lookRight || rb.velocity.x > 0 && lookRight)
       {
          Flip();
       }
    } 

    private void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);
            
            if (dist < attackRange & Time.time - lastAttackTime >= attaclRate)
            {
             	lastAttackTime= Time.time;
             	anim.SetTrigger("attack");
                rb.velocity = Vector2.zero;
            }
            else if (dist > attackRange)
            {
                if (path == null)
                {
                    return;
                }

                if (currentWayPoint >= path.vectorPath.Count)
                {
                    reachEndPath = true;
                }
                else
                {
                    reachEndPath = false;
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.fixedDeltaTime;
                
                rb.velocity = force;
                
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
                if (distance < nextWaypointDistance)
                {
                    currentWayPoint++;
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        
        DetectPlayer();
    }

    void DetectPlayer() //calcule la distance entre nous et l ennemi 
    {
        if (Time.time - playerDetectTime > playerDetectRate)
        {
            playerDetectTime = Time.time;

            foreach (perso_principal player in FindObjectsOfType<perso_principal>())
            {
                if (player != null)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);
                    if (player == targetPlayer)
                    {
                        if (dist > chaseRange)
                    {
                            targetPlayer = null;
                      rb.velocity= Vector2.zero;
                      anim.SetBool("onMove", false);
                    }

             
                    }
                    else if (dist < chaseRange)
                    {
                        if (targetPlayer == null)                
                            targetPlayer = player;
                   		anim.SetBool("onMove",true);
                    }
                }
            }
        }
    }
    void Flip()
    {
       lookRight = !lookRight;
       transform.Rotate(0, 180f, 0);
    }

    void Attack()
    {
       Collider2D player = Physics2D.OverlapCircle(attackPoint.transform.position,0.5f, playerLayerMask);
       if (player != null  && player.tag== "Player")
       {
          player.GetComponent<perso_principal>().TakeDamage(damage);
          
       }
    }
}