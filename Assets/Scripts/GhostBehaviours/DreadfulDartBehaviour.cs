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

        //float timePassed = 0f;
        //float currAngle = Vector2.SignedAngle(rb.transform.up, targetPosition);
        targetPosition = player.transform.position;

        //while (timePassed < timeBeforeShoot)
        //{

        //    float factor = timePassed / timeBeforeShoot;
        //    float angle = Vector2.SignedAngle(rb.transform.up, targetPosition);

        //    rb.rotation = angle;
        //    currAngle = angle;
        //    timePassed += Time.deltaTime;

        //    yield return null;
        //}
        yield return new WaitForSecondsRealtime(timeBeforeShoot);
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        rb.velocity = (targetPosition - currPos).normalized * moveSpeed;
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
                // Start coroutine here
                StartCoroutine(Shoot());
            }
            else
                rb.velocity = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized * moveSpeed;
        }

    }
}
