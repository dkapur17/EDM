using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicController : MonoBehaviour
{
    public GameObject player;
    public GameObject spawnFrom;
    public GameObject vizCrosshair;

    private GameObject mic;
    private GameObject crosshair;
    private Vector3 direction;
    
    public float micSpeed;
    public bool micInUse;
    public bool micReturning;
    private bool ghostCaptured;
    private float eps;

    private float screenHalfWidth;
    private float screenHalfHeight;

    private LineRenderer lr;
    private Transform[] points;


    // Start is called before the first frame update
    void Start()
    {
        mic = this.transform.GetChild(0).gameObject;
        mic.GetComponent<Renderer>().enabled = false;
        mic.GetComponent<Collider2D>().enabled = false;
        
        crosshair = GameObject.Instantiate(vizCrosshair);

        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;

        micInUse = false;
        micSpeed = 10.0f;
        micReturning = false;
        eps = 0.1f;

        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Crosshair update
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        crosshair.transform.position = new Vector2(target.x, target.y);
        
        // Microphone control logic
        if(Input.GetKeyDown(KeyCode.Mouse0) && !micInUse) {
            micInUse = true;
            Vector3 spawnPos = spawnFrom.transform.position;
            direction = Vector3.Normalize(crosshair.transform.position - spawnFrom.transform.position);
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            spawnMic(spawnPos, rotationZ);
        }

        if(micInUse) {
            if(mic.transform.position.x < -screenHalfWidth || mic.transform.position.x > screenHalfWidth
            || mic.transform.position.y < -screenHalfHeight || mic.transform.position.y > screenHalfHeight){
                micReturning = true;
            }

            if(micReturning){
                direction = Vector3.Normalize(spawnFrom.transform.position-mic.transform.position);
                if(Vector3.Distance(mic.transform.position, spawnFrom.transform.position) < eps){
                    mic.GetComponent<Renderer>().enabled = false;
                    mic.GetComponent<Collider2D>().enabled = false;
                    micInUse = false;
                    lr.enabled = false;
                    micReturning = false;
                }
            }

            Vector3 newPos = mic.transform.position + Time.deltaTime * micSpeed * direction;
            mic.transform.position = newPos;

            lr.SetPosition(0, spawnFrom.transform.position);
            lr.SetPosition(1, newPos);

        }
    }

    void spawnMic(Vector3 start, float rotationZ){
            mic.GetComponent<Renderer>().enabled = true;
            mic.GetComponent<Collider2D>().enabled = true;
            lr.enabled = true;
            mic.transform.position = start;
            mic.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            ghostCaptured = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(ghostCaptured)
            return;
        if(other.tag == "Ghost"){
            Debug.Log("Ghost captured");
            player.GetComponent<PlayerController>().ChargeHeadphones();
            ghostCaptured = true;
            if(!micReturning)
                micReturning = true;
            Destroy(other.gameObject);
        }
    }
    // private void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log("Colided");
    //     Debug.Log(other.gameObject);    
    // }
}
