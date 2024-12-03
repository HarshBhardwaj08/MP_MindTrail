using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f; 
    public Transform leftBoundary; 
    public Transform rightBoundary; 

    [Header("Combat Settings")]
    public Transform player; 
    public float fireballRange = 10f; 
    public float swordAttackRange = 2f; 
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint; 
    public float fireballForce = 10f; 
    public float fireballCooldown = 2f; 
    public float swordAttackCooldown = 1.5f; 

    [Header("Components")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool movingRight = true;
    private bool isAttacking = false;
    private float fireballTimer = 0f;
    private float swordAttackTimer = 0f;
    public int enemyHealth = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        
    }

    public override void OnDamage()
    {
        Debug.Log("Damage Deducted");
        animator.SetTrigger("Death");
    }
    public void DisableObject()
    {  
        EnableHealthPickup();
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
      
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= swordAttackRange)
        {
            SwordAttack();
        }
        else if (distanceToPlayer <= fireballRange)
        { 
            FacePlayer();
            ThrowFireball();
        }
        else
        {
            Patrol();
        }

        UpdateAnimation(distanceToPlayer);
    }
    void FacePlayer()
    {
        // Flip the enemy to face the player based on the player's position
        if (player.position.x > transform.position.x && !movingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && movingRight)
        {
            Flip();
        }
    }
    void Patrol()
    {
        if (isAttacking) return;

        // Move enemy
        float direction = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * moveSpeed*Time.deltaTime, rb.velocity.y);

        // Check boundaries
        if (movingRight && transform.position.x > rightBoundary.position.x)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x < leftBoundary.position.x)
        {
            Flip();
        }
    }

    void ThrowFireball()
    {
        if (isAttacking || fireballTimer > 0f) return;

        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetTrigger("throwFireball");

      
        SpawnFireball();

        // Cooldown
        fireballTimer = fireballCooldown;
        Invoke(nameof(ResetAttack), 0.9f); 
    }

    void SpawnFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
        fireball.transform.localScale =  movingRight == true ? new Vector3(1f, 1f, 1f):new Vector3 (-1f, 1f, 1f);
     
        Vector2 fireballDirection = (player.position - fireballSpawnPoint.position).normalized;
       
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        fireballRb.AddForce(fireballDirection * fireballForce*Time.deltaTime, ForceMode2D.Impulse);
    }

    void SwordAttack()
    {
        if (isAttacking || swordAttackTimer > 0f) return;

        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetTrigger("SwordAttack");

        // Cooldown
        swordAttackTimer = swordAttackCooldown;
        Invoke(nameof(ResetAttack), 0.5f); // Allow animations to transition
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the x-scale
        transform.localScale = scale;
    }

    void UpdateAnimation(float distanceToPlayer)
    {
        bool isWalking = !isAttacking && Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool("Walk", isWalking);
       // animator.SetBool("isPlayerNearby", distanceToPlayer <= fireballRange);
    }

    private void FixedUpdate()
    {
        // Handle cooldowns
        if (fireballTimer > 0f) fireballTimer -= Time.deltaTime;
        if (swordAttackTimer > 0f) swordAttackTimer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        // Draw patrol boundaries
        if (leftBoundary != null && rightBoundary != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(leftBoundary.position, rightBoundary.position);
        }

        // Draw combat ranges
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fireballRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, swordAttackRange);
    }
}
