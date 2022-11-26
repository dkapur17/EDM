using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicController : MonoBehaviour
{
    public GameObject player;
    public Transform spawnFrom;
    public GameObject vizCrosshair;

    private GameObject mic;
    private GameObject crosshair;
    private Vector3 direction;
    
    public float micSpeed;
    private bool micInUse;
    private bool micReturning;
    private bool ghostCaptured;
    private float eps;

    private float screenHalfWidth;
    private float screenHalfHeight;

    private Dictionary<string, int> scores = new Dictionary<string, int>();

    public Scores display;



    // Start is called before the first frame update
    void Start()
    {
        mic = transform.GetChild(0).gameObject;
        mic.SetActive(false);
        
        crosshair = Instantiate(vizCrosshair);

        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;

        micInUse = false;
        micSpeed = 10.0f;
        micReturning = false;
        eps = 0.1f;

        Cursor.visible = false;

        scores.Add("GuidedGhoul", 0);
        scores.Add("WistfulWanderer",0);
        scores.Add("DreadfulDart",0);
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
            Vector3 spawnPos = spawnFrom.position;
            direction = Vector3.Normalize(crosshair.transform.position - spawnFrom.position);
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            spawnMic(spawnPos, rotationZ);
        }

        if(micInUse) {
            if(mic.transform.position.x < -screenHalfWidth || mic.transform.position.x > screenHalfWidth
            || mic.transform.position.y < -screenHalfHeight || mic.transform.position.y > screenHalfHeight){
                micReturning = true;
            }

            if(micReturning){
                direction = Vector3.Normalize(spawnFrom.position-mic.transform.position);
                if(Vector3.Distance(mic.transform.position, spawnFrom.position) < eps){
                    mic.SetActive(false);
                    micInUse = false;
                    micReturning = false;
                }
            }

            Vector3 newPos = mic.transform.position + Time.deltaTime * micSpeed * direction;
            Vector3 newDirection = Vector3.Normalize(newPos - spawnFrom.position);
            float rotationZ = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            
            mic.transform.position = newPos;
            mic.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            
            mic.GetComponent<LineRenderer>().SetPosition(0, spawnFrom.position);
            mic.GetComponent<LineRenderer>().SetPosition(1, newPos);

        }
    }

    void spawnMic(Vector3 start, float rotationZ){
        mic.SetActive(true);
        mic.transform.position = start;
        mic.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        ghostCaptured = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(ghostCaptured)
            return;
        if(other.tag == "Ghost"){
            string keyToUpdate = "None";
            
            foreach(KeyValuePair<string, int> kvp in scores)
                if(other.gameObject.name.Contains(kvp.Key))
                    keyToUpdate = kvp.Key;
            if(keyToUpdate != "None")
                scores[keyToUpdate] += 1;
            display.updateScores(scores);

            player.GetComponent<PlayerController>().ChargeHeadphones();
            ghostCaptured = true;
            if(!micReturning)
                micReturning = true;
            
            Destroy(other.gameObject);
        }
    }
}
