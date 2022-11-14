using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GhostSpawner : MonoBehaviour
{

    public GroundVisualizer groundVisualizer;
    public GameObject ghost;
    public GameObject dartGhost;
    public GameObject wanderGhost;
    public GameObject homingGhost;
    public float spawnThreshold = 7.3f;
    
    private float screenHalfWidth;
    private float screenHalfHeight;

    private float freqSum;
    private float prevSum = 0f;
    
    public float ghostCoolDownPeriod = 5.0f;
    private float ghostTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ghostTimer += Time.deltaTime;
        freqSum = groundVisualizer.spectrumData.Sum();
        if(freqSum > spawnThreshold && prevSum != freqSum && ghostTimer > ghostCoolDownPeriod)
        {
            GameObject ghostClone;
            // Debug.Log(freqSum);

            float x = Random.Range(-screenHalfWidth, screenHalfWidth);
            float y = Random.Range(0, screenHalfHeight);
            
            // spawn ghost
            float whichGhost = Random.Range(0f, 1f);
            if (whichGhost < 0.4f)
            {
                // spawn ghost 1
                x = Random.Range(-screenHalfWidth, -screenHalfWidth+2*screenHalfWidth/3);
                y = Random.Range(screenHalfHeight/3, screenHalfHeight);
                ghostClone = GameObject.Instantiate(dartGhost);
                InitGhost(ghostClone, x, y);
                // spawn ghost 2
                x = Random.Range(-screenHalfWidth+2*screenHalfWidth/3, -screenHalfWidth+4*screenHalfWidth/3);
                y = Random.Range(screenHalfHeight/3, screenHalfHeight);
                ghostClone = GameObject.Instantiate(dartGhost);
                InitGhost(ghostClone, x, y);
                // spawn ghost 3
                x = Random.Range(-screenHalfWidth+4*screenHalfWidth/3, screenHalfWidth);
                y = Random.Range(screenHalfHeight/3, screenHalfHeight);
                ghostClone = GameObject.Instantiate(dartGhost);
                InitGhost(ghostClone, x, y);
            }
            else if (whichGhost < 0.7f)
            {
                // spawn ghost 1
                x = Random.Range(-screenHalfWidth, 0);
                y = Random.Range(screenHalfHeight/3, screenHalfHeight);
                ghostClone = GameObject.Instantiate(wanderGhost);
                InitGhost(ghostClone, x, y);
                // spawn ghost 2
                x = Random.Range(screenHalfHeight/3, screenHalfWidth);
                y = Random.Range(screenHalfHeight/3, screenHalfHeight);
                ghostClone = GameObject.Instantiate(wanderGhost);
                InitGhost(ghostClone, x, y);
            }
            else
            {
                // spawn ghost 1
                x = Random.Range(-screenHalfWidth, screenHalfWidth);
                y = Random.Range(screenHalfHeight/3, screenHalfHeight);
                ghostClone = GameObject.Instantiate(homingGhost);
                InitGhost(ghostClone, x, y);
            }

            ghostTimer = 0.0f;
        }
        prevSum = freqSum;
    }

    void InitGhost(GameObject ghostClone, float x, float y)
    {
        // set spawning position
        Vector2 ghostPos = new Vector2(x, y);
        ghostClone.transform.position = ghostPos;
        ghostClone.transform.parent = transform;

        // get attributes of ghost
        GhostBehavior gb = ghostClone.GetComponent<GhostBehavior>();
    }
}
