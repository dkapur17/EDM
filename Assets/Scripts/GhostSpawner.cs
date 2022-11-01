using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GhostSpawner : MonoBehaviour
{

    public GroundVisualizer groundVisualizer;
    public GameObject ghost;
    public float spawnThreshold = 7.3f;
    private int numGhosts = 5;
    
    private List<GameObject> ghosts = new List<GameObject>(0);
    private float screenHalfWidth;
    private float screenHalfHeight;

    private float freqSum;
    private float prevSum = 0f;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHalfHeight = Camera.main.orthographicSize;

        // for (int i = 0; i < numGhosts; i++)
        // {
        //     // spawn ghost
        //     GameObject ghostClone = GameObject.Instantiate(ghost);
        //     ghosts.Add(ghostClone);

        //     // set spawning position
        //     Vector2 ghostPos = new Vector2(screenHalfWidth, screenHalfHeight-i*screenHalfHeight/numGhosts);
        //     ghostClone.transform.position = ghostPos;
        //     ghostClone.transform.parent = transform;

        //     GhostBehavior gb = ghostClone.GetComponent<GhostBehavior>();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        freqSum = groundVisualizer.spectrumData.Sum();
        if(freqSum > spawnThreshold && prevSum != freqSum)
        {
            Debug.Log(freqSum);

            // spawn ghost
            GameObject ghostClone = GameObject.Instantiate(ghost);
            ghosts.Add(ghostClone);

            // set spawning position
            Vector2 ghostPos = new Vector2(screenHalfWidth, screenHalfHeight);
            ghostClone.transform.position = ghostPos;
            ghostClone.transform.parent = transform;

            GhostBehavior gb = ghostClone.GetComponent<GhostBehavior>();
        }
        prevSum = freqSum;
    }
}
