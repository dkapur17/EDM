using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public int extraJumps;

    private float moveInput;
    private int jumpsLeft;


    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = extraJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
            jumpsLeft = extraJumps;

        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpsLeft > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpsLeft--;
        }
        else if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpsLeft == 0 && isGrounded)
            rb.velocity = Vector2.up * jumpForce;
        
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
}
