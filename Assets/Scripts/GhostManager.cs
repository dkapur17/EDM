using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostManager : MonoBehaviour
{

    public enum AudioClips
    {
        Narco,
        YouAndI,
        ByYourSide,
        Dioma,
        Ego,
        Redemption,
        Samurai,
        WarMachine,
    };


    public GroundVisualizer groundVisualizer;
    public float spawnThreshold = 7.3f;
    public float maxSpectralSum = 10f;
    public List<GameObject> ghostTypes;
    public AudioClips currentLevelAudio;


    public Vector2 waveSize;
    public Vector2 waveDelay;

    private List<Vector2> spawnCentres = new List<Vector2>();
    private Vector2 screenSize;

    private Dictionary<AudioClips, float> audioMaxSum = new Dictionary<AudioClips, float>();

    private float timeToNextWave;
    private float maxSum;
    private bool spawnable=true;

    private GameObject[] ghosts;
    // Start is called before the first frame update
    void Start()
    {

        audioMaxSum.Add(AudioClips.Narco, 106f);
        audioMaxSum.Add(AudioClips.YouAndI, 112f);
        audioMaxSum.Add(AudioClips.ByYourSide, 128f);
        audioMaxSum.Add(AudioClips.Dioma, 116.0f);
        audioMaxSum.Add(AudioClips.Ego, 112f);
        audioMaxSum.Add(AudioClips.Redemption, 100f);
        audioMaxSum.Add(AudioClips.Samurai, 81f);
        audioMaxSum.Add(AudioClips.WarMachine, 107f);
        
        
        
        
       
        GameObject[] bounds =  GameObject.FindGameObjectsWithTag("Bound");
        foreach(GameObject bound in bounds)
            spawnCentres.Add(new Vector2(bound.transform.position.x, bound.transform.position.y));

        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        timeToNextWave = Random.Range(waveDelay.x, waveDelay.y);

        maxSum = audioMaxSum[currentLevelAudio];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToNextWave -= Time.fixedDeltaTime;

        if(spawnable && timeToNextWave <= 0)
        {
            float currSum = groundVisualizer.spectrumData.Select(x => Mathf.Abs(x)).Sum();

            int numGhosts = (int)((waveSize.y - waveSize.x) * (currSum/maxSum) + waveSize.x);

            for(int i = 0; i < numGhosts; i++)
            {
                GameObject ghostType = ghostTypes[Random.Range(0, ghostTypes.Count)];
                GameObject ghost = GameObject.Instantiate(ghostType, transform);
                ghost.transform.position = GenerateSpawnPoint();
            }

            timeToNextWave = Random.Range(waveDelay.x, waveDelay.y);
        }

    }

    private Vector2 GenerateSpawnPoint()
    {
        Vector2 spawnSite = spawnCentres[Random.Range(0, spawnCentres.Count)];

        // Left or Right
        if (spawnSite.x == 0)
        {
            spawnSite.x += Random.Range(-screenSize.x / 2, screenSize.x / 2);
            spawnSite.y += Random.Range(-2, 2);
        }
        else
        {
            spawnSite.x += Random.Range(-2, 2);
            spawnSite.y += Random.Range(-screenSize.y / 2, screenSize.y / 2);
        }

        return spawnSite; 
    }

    public void setSpawnable(bool newSpawnable) {
        if(!newSpawnable){
            ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghost in ghosts)
                ghost.GetComponent<GhostFade>().FadeOut();
             
        }
        spawnable = newSpawnable;
    }
}
