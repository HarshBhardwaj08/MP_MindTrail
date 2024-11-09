using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;           // Speed of movement
    public float jumpForce = 10f;          // Jump force
    public Transform groundCheck;          // Point from where raycast will start
    public float groundCheckDistance = 0.2f;  // Distance of the raycast to check ground
    public LayerMask groundLayer;          // Layer of the ground to check for collisions
    public static event Action<int> OnPlayerJump;
    private Rigidbody2D rb;                // Reference to Rigidbody2D component
    public Animator animator;             // Reference to Animator component
    private float moveInput;               // Horizontal movement input
    private bool isGrounded;               // To check if the player is grounded

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        DialogueManager.OnDialogue += ResetMovement;
    }

    private void ResetMovement()
    {
       
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogue -= ResetMovement;
    }
    void Update()
    {
        // Movement input (A, D or left arrow, right arrow)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Move the player
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Check if the player is grounded using a raycast
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // Jumping logic

        if (isGrounded&&Input.GetKey(KeyCode.Space))
        {
          //  animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            OnPlayerJump?.Invoke(5);
        }

        // Update animation parameters
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the player when moving left or right
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Update the jump animation
        if (isGrounded)
        {
           // animator.SetBool("IsJumping", false);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a ray in the Scene view for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}
