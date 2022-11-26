using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public int extraJumps;

    [Header("Headphones")]
    public int headphoneCharge=0;
    public int maxCharge=8;
    public float timePerCharge=0.25f;
    public float timeToFade = 1f;
    public Bar headphones;

    [Header("Sprite Switching")]
    public Sprite headWithHeadphones;
    public Sprite headWithoutHeadphones;
    public Sprite torsoWithHeadphones;
    public Sprite torsoWithoutHeadphones;

    public SpriteRenderer headSpriteRenderer;
    public SpriteRenderer torsoSpriteRenderer;

    [Header("Interactions")]
    public int livesLeft = 5;
    public float checkRadius;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public GameObject ghostManager;
    public Bar healthbar;

    private float moveInput;
    private int jumpsLeft;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    private bool facingRight = true;
    private bool headphoneActive=false;

    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = extraJumps;

        audioSource = GameObject.Find("Ground").GetComponent<AudioSource>();

        headSpriteRenderer.sprite = headWithoutHeadphones;
        torsoSpriteRenderer.sprite = torsoWithHeadphones;

        healthbar.SetMaxValue(livesLeft);
        headphones.SetMaxValue(maxCharge);
        headphones.SetValue(0);

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
        if(col.tag != "Ghost")
            return;
        Destroy(col.gameObject);
        livesLeft--;
        healthbar.SetValue(livesLeft);
        if (livesLeft <= 0)
        {
            // redirect to main menu
            // Debug.Log("Game Ended");
        }
    }

    private IEnumerator UseHeadphones() {

        // Sprite Updates
        headSpriteRenderer.sprite = headWithHeadphones;
        torsoSpriteRenderer.sprite = torsoWithoutHeadphones;
        animator.SetTrigger("putOnHeadphones");
        // Disable Ghost Spawning
        ghostManager.GetComponent<GhostManager>().setSpawnable(false);

        // Mute
        float timeElapsed = 0f;
        while(timeElapsed < timeToFade)
        {
            audioSource.volume -= 0.1f;
            timeElapsed += timeToFade / 10;
            yield return new WaitForSecondsRealtime(timeToFade / 10);
        }

        audioSource.volume = 0f;
        audioSource.mute = true;

        // Wait for charge to run out
        while(headphoneCharge != 0){
            yield return new WaitForSecondsRealtime(timePerCharge);
            headphoneCharge--;
            headphones.SetValue(headphoneCharge);
        }
        headphoneCharge = 0;
        headphoneActive = false;

        // Sprite Updates
        headSpriteRenderer.sprite = headWithoutHeadphones;
        torsoSpriteRenderer.sprite = torsoWithHeadphones;
        animator.SetTrigger("takeOffHeadphones");

        // Unmute
        timeElapsed = 0f;
        while (timeElapsed < timeToFade){
            audioSource.volume += 0.1f;
            timeElapsed += timeToFade / 10;
            yield return new WaitForSecondsRealtime(timeToFade / 10);
        }
        audioSource.volume = 1.0f;
        audioSource.mute = false;

        // Enable Ghost Spawning
        ghostManager.GetComponent<GhostManager>().setSpawnable(true);

    }

    public void ChargeHeadphones() {
        headphoneCharge = Mathf.Min(maxCharge, headphoneCharge+1);
        headphones.SetValue(headphoneCharge);
    }
}