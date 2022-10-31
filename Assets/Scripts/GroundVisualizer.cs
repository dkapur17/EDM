using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundVisualizer : MonoBehaviour
{

    public GameObject vizTile;
    public int numTiles = 16;
    public float tileGap = 0.1f;
    public float tileHeightShow = 0.1f;
    public float lerpFactor = 0.5f;

    public int updateEvery = 10;
    private int frameIndex = 0;

    private List<GameObject> tiles = new List<GameObject>(0);
    private float[] offsets;
    
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        offsets = new float[numTiles];
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float screenHalfHeight = Camera.main.orthographicSize;
        float blockWidth = (screenHalfWidth * 2) / numTiles;
        float tileWidth = blockWidth - 2 * tileGap;

        for (float x = -screenHalfWidth + blockWidth/2; x < screenHalfWidth + blockWidth/2; x+= blockWidth)
        {
            GameObject tile = GameObject.Instantiate(vizTile);

            tiles.Add(tile);

            Vector3 scale = tile.transform.localScale;
            scale.x = tileWidth;
            scale.y = screenHalfHeight;
            tile.transform.localScale = scale;

            Vector2 tilePos = new Vector2(x, transform.position.y - screenHalfHeight/2 + tileHeightShow);
            tile.transform.position = tilePos;

            tile.transform.parent = transform;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float screenHalfHeight = Camera.main.orthographicSize;

        if (frameIndex % updateEvery == 0)
        {
            float[] spectrumData = new float[4 * numTiles];
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

            for (int i = 0; i < 2*numTiles; i++)
                spectrumData[i] += spectrumData[4*numTiles - i - 1];

            for (int i = 0; i < numTiles; i++)
                spectrumData[i] += spectrumData[2 * numTiles - i - 1];

            for (int i = 0; i < numTiles / 2; i++)
            {
                spectrumData[i] += spectrumData[numTiles - i - 1];
                spectrumData[numTiles - i - 1] = spectrumData[i];
            }

            for (int i = 0; i < numTiles; i++)
            {
                //float offsetVal = Random.Range(-1f, 1f) * screenHalfHeight / 2 - 1.5f * screenHalfHeight + tileHeightShow;
                //float offsetVal = Random.Range(-1.5f * screenHalfHeight + tileHeightShow, -1.1f * screenHalfHeight); 
                float minHeight = -1.5f * screenHalfHeight + tileHeightShow;
                float maxHeight = -1.2f * screenHalfHeight;

                float offsetVal = minHeight + (spectrumData[i] * (maxHeight - minHeight) * 3);
                offsets[i] = offsetVal;
            }

            System.Random r = new System.Random();
            offsets = offsets.OrderBy(x => r.Next()).ToArray();

            if (frameIndex != 0)
                frameIndex = 0;
        }

        for (int i = 0; i < numTiles; i++)
        {
            Rigidbody2D rb = tiles[i].GetComponent<Rigidbody2D>();
            float targetPos = Mathf.Lerp(tiles[i].transform.position.y, offsets[i], lerpFactor);
            rb.MovePosition(new Vector2(tiles[i].transform.position.x, targetPos));
        }

        frameIndex++;
        frameIndex %= updateEvery;
    }
}
