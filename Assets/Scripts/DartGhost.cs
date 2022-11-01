using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartGhost : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float timeRemaining = 5f;
    public float timeBeforeStart = 1f;

    private float screenHalfWidth;
    private float screenHalfHeight;

    private GameObject player;
    private Vector2 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;

        rb = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);

        StartCoroutine(Move());
        // Destroy(gameObject, timeRemaining);
    }

    IEnumerator Move()
    {
        yield return new WaitForSecondsRealtime(timeBeforeStart);
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        rb.velocity = (playerPosition - currPos).normalized * moveSpeed;
    }

    void Update() 
    {
        // Check location. If out of bounds, destroy gameobject
        float x = transform.position.x;
        float y = transform.position.y;

        if (x < -screenHalfWidth || x > screenHalfWidth)
            Destroy(gameObject);
        if (y < -screenHalfHeight || y > screenHalfHeight)
            Destroy(gameObject);
    }
}
