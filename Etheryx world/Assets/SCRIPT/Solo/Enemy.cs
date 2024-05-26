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
    private bool onMove;

    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] float damage;
    [SerializeField] float attackRate;
    private float lastAttackTime;
    public Transform attackPoint;
    public LayerMask playerLayerMask;
    private bool isAttacking; // Nouveau booléen pour gérer l'état d'attaque
    private bool wasattacking; // Nouveau booléen pour indiquer si l'ennemi attaquait précédemment
    public float attackInterval = 0.3f; // Intervalle fixe entre les coups
    public float minAttackDuration = 1f; // Durée minimum de l'attaque
    public float maxAttackDuration = 3f; // Durée maximum de l'attaque

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

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        previousPosition = rb.position;
        smoothH = 0f;
        smoothV = 0f;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
            pathPending = false; // Réinitialise l'état de la demande de chemin
        }
        else
        {
            pathPending = false; // Réinitialise l'état de la demande de chemin même en cas d'erreur
        }
    }

    void UpdatePath()
    {
        if (!pathPending && seeker.IsDone() && targetPlayer != null)
        {
            pathPending = true; // Marque qu'une demande de chemin est en attente
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }

    private void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            CheckIfBlocked(); // Vérifie si l'ennemi est bloqué

            if (isBlocked)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("onMove", false);
                return;
            }

            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                lastAttackTime = Time.time;
                isAttacking = true; // Définir l'état d'attaque
                rb.velocity = Vector2.zero;
                anim.SetBool("onMove", false);
                anim.SetTrigger("attack");
                StartCoroutine(PerformAttacks()); // Attaquer avec des intervalles
                return;
            }
            else if (dist > attackRange)
            {
                if (isAttacking) return; // Sortir si l'ennemi est en train d'attaquer

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
                rb.velocity = Vector2.zero;
                anim.SetBool("onMove", false);
            }

            // Lissage des valeurs de H et V
            smoothH = Mathf.Lerp(smoothH, Mathf.Clamp(moveX, -1f, 1f), smoothingFactor);
            smoothV = Mathf.Lerp(smoothV, Mathf.Clamp(moveY, -1f, 1f), smoothingFactor);

            anim.SetFloat("H", smoothH);
            anim.SetFloat("V", smoothV);
        }

        DetectPlayer();
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
            yield return new WaitForSeconds(attackInterval); // Utilise attackInterval ici
        }

        isAttacking = false; // Réinitialise l'état d'attaque une fois la série d'attaques terminée
        anim.SetBool("onMove", true); // Assurez-vous que l'animation de mouvement est réactivée
        wasattacking = true; // Indiquer que l'ennemi a attaqué
    }

    void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayerMask);
        foreach (Collider2D player in players)
        {
            if (player.tag == "Player")
            {
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
}
