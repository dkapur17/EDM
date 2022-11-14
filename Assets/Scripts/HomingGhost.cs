using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingGhost : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float timeRemaining = 5f;
    public float timeBeforeStart = 1f;

    private float screenHalfWidth;
    private float screenHalfHeight;

    private GameObject player;
    private Vector2 followPosition;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;

        rb = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        followPosition = new Vector2(player.transform.position.x, player.transform.position.y);

        StartCoroutine(Move());
        InvokeRepeating("PlanTrajectory", 2.0f, 0.05f);
        // Destroy(gameObject, timeRemaining);
    }

    IEnumerator Move()
    {
        yield return new WaitForSecondsRealtime(timeBeforeStart);
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        rb.velocity = (followPosition - currPos).normalized * moveSpeed;
    }

    void PlanTrajectory()
    {
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        followPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 des_vel = (followPosition - currPos).normalized;
        Vector2 cur_vel = rb.velocity.normalized;
        cur_vel = cur_vel + 0.5f * (des_vel - cur_vel);
        rb.velocity = cur_vel * moveSpeed;
    }

    void FixedUpdate() 
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
