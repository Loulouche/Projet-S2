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
    public float Maxvie;
    public float Vie;

    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] float damage;
    [SerializeField] float attackRate;
    private float lastAttackTime;
    public Transform attackPoint;
    public LayerMask playerLayerMask;
    private bool isAttacking; 
    private bool wasattacking; 
    public float attackInterval = 0.3f; 
    public float minAttackDuration = 1f; 
    public float maxAttackDuration = 3f; 

    [Header("Component")]
    Rigidbody2D rb;
    private perso_principal targetPlayer;
    Animator anim;
    BoxCollider2D boxCollider;

    [Header("Pathfinding")]
    public float nextWaypointDistance = 2f;
    Path path;
    int currentWayPoint = 0;
    bool reachEndPath = false;
    Seeker seeker;

    // Variables pour détecter et gérer les blocages
    private Vector2 previousPosition;
    private float blockedTime = 0f;
    private bool isBlocked = false;
    public float blockedThreshold = 0.2f; // Temps en secondes avant de considérer que l'ennemi est bloqué
    public float recalculationInterval = 1f; // Intervalle pour recalculer le chemin

    // Variables pour les coordonnées du vecteur de déplacement
    private float moveX;
    private float moveY;

    // Variables pour les valeurs lissées de H et V
    private float smoothH;
    private float smoothV;
    public float smoothingFactor = 0.1f; // Facteur de lissage

    private bool pathPending = false; // Nouveau booléen pour suivre l'état de la demande de chemin


    private Vector2 startPosition;


   
    void Start()
    {
        Vie = Maxvie;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        previousPosition = rb.position;
        smoothH = 0f;
        smoothV = 0f;
        InvokeRepeating("UpdatePath", 0f, .5f);
        
        startPosition = transform.position; // Enregistrement de la position initiale
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
            pathPending = false; // Réinitialiser l'état de la demande de chemin
        }
        else
        {
            pathPending = false; // Réinitialiser même en cas d'erreur
        }
    }

    void UpdatePath()
    {
        if (!pathPending && seeker.IsDone() && targetPlayer != null)
        {
            pathPending = true; // Marquer qu'une demande de chemin est en attente
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }

   
private void FixedUpdate()
{
    if (targetPlayer != null)
    {
        // Vérifie si l'ennemi est bloqué
        CheckIfBlocked();

        if (isBlocked)
        {
            // Si l'ennemi est bloqué, arrêtez son mouvement
            rb.velocity = Vector2.zero;
            anim.SetBool("onMove", false);
            return;
        }

        float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

        if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
        {
            // Si l'ennemi est assez proche pour attaquer et qu'il peut attaquer
            lastAttackTime = Time.time;
            isAttacking = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("onMove", false);
            anim.SetTrigger("attack");
            StartCoroutine(PerformAttacks());
            return;
        }
        else if (dist > attackRange)
        {
            // Si l'ennemi est trop loin pour attaquer, gérer le mouvement vers la cible
            if (isAttacking) return;

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

                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.fixedDeltaTime;

                rb.velocity = force;

                moveX = direction.x;
                moveY = direction.y;

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
                if (distance < nextWaypointDistance)
                {
                    currentWayPoint++;
                }
                anim.SetBool("onMove", true);
            }
        }
        else
        {
            // Si l'ennemi est dans la plage d'attaque mais ne peut pas attaquer, arrêtez son mouvement
            rb.velocity = Vector2.zero;
            anim.SetBool("onMove", false);
        }

        // Lissage des valeurs de H et V pour les animations
        smoothH = Mathf.Lerp(smoothH, Mathf.Clamp(moveX, -1f, 1f), smoothingFactor);
        smoothV = Mathf.Lerp(smoothV, Mathf.Clamp(moveY, -1f, 1f), smoothingFactor);

        anim.SetFloat("H", smoothH);
        anim.SetFloat("V", smoothV);
    }

    DetectPlayer(); // Méthode pour détecter le joueur à intervalles réguliers
}

    void DetectPlayer()
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
                            rb.velocity = Vector2.zero;
                            anim.SetBool("onMove", false);
                        }
                    }
                    else if (dist < chaseRange)
                    {
                        if (targetPlayer == null)
                            targetPlayer = player;
                        anim.SetBool("onMove", true);
                    }
                }
            }
        }
    }

    void CheckIfBlocked()
    {
        if ((Vector2)transform.position == previousPosition && !wasattacking)
        {
            blockedTime += Time.fixedDeltaTime;
            if (blockedTime > blockedThreshold)
            {
                Debug.Log("Ennemi bloqué !");
                isBlocked = true;
                InvokeRepeating("TryUnblock", 0f, recalculationInterval);
            }
        }
        else
        {
            blockedTime = 0f;
            previousPosition = transform.position;
        }
    }
    
    void TryUnblock()
    {
        if (targetPlayer != null && !pathPending)
        {
            pathPending = true;
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }

        if (seeker.IsDone() && path != null && path.vectorPath.Count > 0)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, chaseRange, ~LayerMask.GetMask("Enemy"));
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                isBlocked = false;
                CancelInvoke("TryUnblock");
                Debug.Log("Ennemi débloqué et reprend la poursuite !");
            }
        }
    }

    IEnumerator PerformAttacks()
    {
        float attackDuration = Random.Range(minAttackDuration, maxAttackDuration);
        float attackEndTime = Time.time + attackDuration;

        while (Time.time < attackEndTime)
        {
            Attack();
            yield return new WaitForSeconds(attackInterval);
        }

        isAttacking = false; 
        anim.SetBool("onMove", true); 
        wasattacking = true; 
    }

    void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayerMask);
    
        foreach (Collider2D player in players)
        {
            if (player.tag == "Player")
            {
                Debug.Log("Ennemi attaque le joueur");
                player.GetComponent<perso_principal>().TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    public void Takedamage(float damage)
    {
        Vie -= damage;
        Debug.Log("Vie après avoir pris des dégâts: " + Vie);
        Debug.Log("Dégâts reçus: " + damage);
        
        if (Vie <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Ajoutez ici toute logique de mort, comme des effets sonores, des animations, etc.
        PlayerPrefs.SetInt("EnemyDead", 1); // Marque l'ennemi comme mort
        gameObject.SetActive(false);
    }
    
    
    
    public void ResetEnemy()
    {
        Vie = Maxvie; // Réinitialiser la vie à sa valeur maximale
        gameObject.SetActive(true);
        transform.position = startPosition; // Réinitialiser la position à la position initiale

        // Réinitialiser les variables d'état pour le mouvement et l'attaque
        isAttacking = false;
        wasattacking = false;
        smoothH = 0f;
        smoothV = 0f;
        
        
    }
    
}