using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public int extraJumps;
    public int livesLeft=3;

    public int headphoneCharge=0;
    public int maxCharge=8;
    public float timePerCharge=0.25f;

    private float moveInput;
    private int jumpsLeft;


    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    private bool facingRight = true;
    private bool headphoneActive=false;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public GameObject ghostManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = extraJumps;

        audioSource = GameObject.Find("Ground").GetComponent<AudioSource>();
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
        
        // Headphone Handling
        if(Input.GetMouseButtonDown(1)) {
            if(!headphoneActive && headphoneCharge != 0){
                headphoneActive = true;
                StartCoroutine(UseHeadphones());
            }
        }
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
            // Debug.Log("Game Ended");
        }
    }

    private IEnumerator UseHeadphones() {
        
        while (audioSource.volume > 0){
            audioSource.volume -= 2 * (Time.deltaTime);
            yield return null;
        }
        audioSource.mute = true;
        ghostManager.GetComponent<GhostManager>().setSpawnable(false);
        yield return new WaitForSecondsRealtime(timePerCharge * headphoneCharge);
        audioSource.mute = false;
        headphoneCharge = 0;
        headphoneActive = false;
        ghostManager.GetComponent<GhostManager>().setSpawnable(true);
        while (audioSource.volume < 1.0f){
            audioSource.volume += 2 * (Time.deltaTime); 
            yield return null;
        }
        audioSource.volume = 1.0f;
    }

    public void ChargeHeadphones() {
        headphoneCharge = Mathf.Min(maxCharge, headphoneCharge+1);
        Debug.Log("Charged headphones" + headphoneCharge.ToString());
    }
}