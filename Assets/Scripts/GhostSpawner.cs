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
    public float spawnThreshold = 7.3f;
    
    private float screenHalfWidth;
    private float screenHalfHeight;

    private float freqSum;
    private float prevSum = 0f;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        freqSum = groundVisualizer.spectrumData.Sum();
        if(freqSum > spawnThreshold && prevSum != freqSum)
        {
            // Debug.Log(freqSum);

            // spawn ghost
            // GameObject ghostClone = GameObject.Instantiate(dartGhost);
            GameObject ghostClone = GameObject.Instantiate(wanderGhost);

            // set spawning position
            float x = Random.Range(-screenHalfWidth, screenHalfWidth);
            float y = Random.Range(0, screenHalfHeight);
            Vector2 ghostPos = new Vector2(x, y);
            ghostClone.transform.position = ghostPos;
            ghostClone.transform.parent = transform;

            // get attributes of ghost
            GhostBehavior gb = ghostClone.GetComponent<GhostBehavior>();
        }
        prevSum = freqSum;
    }
}
