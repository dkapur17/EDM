using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    public float fallThroughDuration = 0.25f;
    private GameObject currentPlatform;
    private CapsuleCollider2D playerCollider;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if(currentPlatform != null)
            {
                animator.SetTrigger("drop");
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            currentPlatform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentPlatform = null;
    }
    

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(fallThroughDuration);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
