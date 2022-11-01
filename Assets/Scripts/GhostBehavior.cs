using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float timeRemaining = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();     
        Destroy(gameObject, timeRemaining);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlanTrajectory();
    }

    void PlanTrajectory()
    {
        rb.velocity = new Vector2(-moveSpeed, 0.0f);
    }
}
