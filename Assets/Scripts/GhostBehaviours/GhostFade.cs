using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public float animationDuration = 0.5f;
    private Animator animator;

    private Rigidbody2D rb;
    private Collider2D col;

    public MonoBehaviour master;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAnimation());
    }

    public void DisableMovement(){
        col.enabled = false;
        rb.velocity = Vector2.zero;
        master.enabled = false;
    }

    IEnumerator FadeAnimation()
    {
        animator.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(animationDuration);
        Destroy(gameObject);
    }
}

