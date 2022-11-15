using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedGhoulBehaviour : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float timeRemaining = 4f;
    public float lerpFactor = 0.001f;
    private bool setToDestroy = false;
    private Vector2 screenSize;

    private GameObject player;

    private Vector2 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindWithTag("Player");

        targetPosition = player.transform.position;

        Vector2 initVelocity = (targetPosition - new Vector2(transform.position.x, transform.position.y));
        if (initVelocity.x < 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = player.transform.position;
        Vector2 velocityToTarget = targetPosition - new Vector2(transform.position.x, transform.position.y);
        rb.velocity = Vector2.Lerp(rb.velocity, velocityToTarget, lerpFactor).normalized * moveSpeed;
        SetRotation();
        if (!setToDestroy)
        {
            if (Mathf.Abs(transform.position.x) < screenSize.x && Mathf.Abs(transform.position.y) < screenSize.y)
            {
                setToDestroy = true;
                Destroy(gameObject, timeRemaining);
            }
        }
    }

    private void SetRotation()
    {
        if (rb.velocity.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
}
