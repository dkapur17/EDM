using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public int extraJumps;
    public int livesLeft=3;

    private float moveInput;
    private int jumpsLeft;


    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = extraJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            jumpsLeft = extraJumps;
            animator.SetBool("isJumping", false);
        }
        else
            animator.SetBool("isJumping", true);

        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumpsLeft > 0)
        {
            animator.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
            jumpsLeft--;
        }
        else if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumpsLeft == 0 && isGrounded)
        {
            animator.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
        }

        // Animation Handling
        animator.SetBool("isRunning", Input.GetAxisRaw("Horizontal") != 0);
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (facingRight && moveInput < 0)
            Flip();
        else if (!facingRight && moveInput > 0)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // called when this GameObject collides with GameObject2.
    void OnTriggerEnter2D(Collider2D col)
    {
        livesLeft--;
        if (livesLeft <= 0)
        {
            // redirect to main menu
            Debug.Log("Game Ended");
        }
    }
}
