using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public float animationDuration = 0.5f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAnimation());
    }

    IEnumerator FadeAnimation()
    {
        animator.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(animationDuration);
        Destroy(gameObject);
    }
}

