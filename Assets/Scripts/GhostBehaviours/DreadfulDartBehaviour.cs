using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadfulDartBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float timeBeforeShoot = 1f;
    public float timeRemaining = 10f;
    public float onScreenPadding = 0.5f;

    private Vector2 screenSize;

    private GameObject player;

    private bool hasShot = false;

    private Vector2 targetPosition;
    private bool flippedX = false;

    // Start is called before the first frame update
    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindWithTag("Player");

        targetPosition = new Vector2(0f, 0f);

        Vector2 initVelocity = (targetPosition - new Vector2(transform.position.x, transform.position.y));
        if (initVelocity.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            flippedX = true;
        }
    }

    IEnumerator Shoot()
    {
        float timePassed = 0f;
        Vector2 targetVec;
        while (timePassed < timeBeforeShoot)
        {
            targetVec = player.transform.position - transform.position;
            float angleToCover = Vector2.SignedAngle(transform.up, targetVec);
            float steps = (timeBeforeShoot - timePassed) / Time.deltaTime;
            rb.rotation += angleToCover/steps;
            timePassed += Time.deltaTime;
            yield return null;
        }
        targetVec = player.transform.position - transform.position;
        rb.velocity = targetVec.normalized * moveSpeed;
        Destroy(gameObject, timeRemaining);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!hasShot)
        {
            if(Mathf.Abs(transform.position.x) < screenSize.x - onScreenPadding && Mathf.Abs(transform.position.y) < screenSize.y - onScreenPadding)
            {
                hasShot = true;
                rb.velocity = Vector2.zero;
                StartCoroutine(Shoot());
            }
            else
                rb.velocity = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized * moveSpeed;
        }

    }
}
