using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Robot : EnemyBase
{
    public Transform player;
    public Transform[] patrolPoints;
    public float detectionRadius = 5f;
    public float attackRadius = 1.5f;
    public float patrolSpeed = 2f;
    public float attackSpeed = 0f;
    public int damage = 10; // Damage dealt to the player
    public float attackCooldown = 1f; // Time between attacks
    public Transform attackPoint; // Point from which raycast originates
    public float attackRange = 1f; // Length of the attack raycast
    public LayerMask playerLayer; // LayerMask for detecting the player

    private int currentPatrolIndex = 0;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = -Mathf.Infinity;
    public static event Action<int> onRobotHit;
    bool IsAlive = true;   
    void Start()
    {
        healthPickup.SetActive(false);
        IsAlive = true;
        animator = GetComponent<Animator>();
        if (patrolPoints.Length > 0)
            transform.position = patrolPoints[0].position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackRadius)
            {
                AttackPlayer();
            }
            else
            {
                MoveTowards(player.position, attackSpeed);
            }
        }
        else
        {
            Patrol();
        }
    }
    public override void OnDamage()
    {
        if(!IsAlive)  return; 
        Debug.Log(this.gameObject.name + "Damage Deducted");
        animator.SetTrigger("Death");
        EnableHealthPickup();
        IsAlive = false ;
    }
    public void DisableObject()
    {   
       
        this.gameObject.SetActive(false);
    }
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        animator.SetBool("isWalking", true);
      //  animator.SetBool("isAttacking", false);

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPoint.position, patrolSpeed);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.localScale = new Vector3(Mathf.Sign(direction.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip sprite
    }

    void AttackPlayer()
    {
        animator.SetBool("isWalking", false);

        if (Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            isAttacking = true;

            // Randomly choose an attack animation
            int attackType = UnityEngine.Random.Range(0, 2); // 0 or 1
            animator.SetTrigger(attackType == 0 ? "Attack1" : "Attack2");

            // Perform raycast after a slight delay to sync with animation
            PerformAttackRaycast();
            lastAttackTime = Time.time;

            Invoke(nameof(ResetAttack), 1f); // Adjust based on animation length
        }
    }

    void PerformAttackRaycast()
    {
        // Cast a ray from the attack point
        //RaycastHit2D hit = Physics2D.Raycast(attackPoint.position,transform.right, attackRange, playerLayer);
        bool isplayerHit = Physics2D.OverlapCircle(attackPoint.position, attackRadius,playerLayer);
        if (isplayerHit == true)
        {
          //  Debug.Log("Player hit by AI attack!");
            onRobotHit?.Invoke(1);
        }

        // Debug visualization
        Debug.DrawRay(attackPoint.position, transform.right * attackRange, Color.red, 0.5f);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius); // Attack range

        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
          //  Gizmos.DrawRay(attackPoint.position, transform.right * attackRange); // Attack ray
        }
    }
}
