using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{

    public float timeToDisappear = 2f;
    public float timeToReappear = 2f;

    private float playerOnPlatformTime;
    // Start is called before the first frame update
    void Start()
    {
        playerOnPlatformTime = 0;
    }

    private void Update()
    {
        Debug.Log(playerOnPlatformTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerOnPlatformTime += Time.deltaTime;

        if(playerOnPlatformTime >= timeToDisappear)
            StartCoroutine(HandlePlatformDisappearance());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerOnPlatformTime = 0;
    }

    private IEnumerator HandlePlatformDisappearance()
    {
        playerOnPlatformTime = 0;
        Disappear();
        yield return new WaitForSecondsRealtime(timeToReappear);
        Reappear();
    }

    private void Disappear()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<PlatformEffector2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Reappear()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<PlatformEffector2D>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
