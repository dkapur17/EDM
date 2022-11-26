using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GroundVisualizer : MonoBehaviour
{

    public GameObject vizTile;
    public int numTiles = 64;
    public float tileGap = 0.1f;
    public float tileHeightShow = 0.1f;
    public float lerpFactor = 0.5f;

    public float[] spectrumData;

    public bool collectSpectralDataForAnalysis = false;

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

        for (float x = -screenHalfWidth + blockWidth/2; x < screenHalfWidth; x+= blockWidth)
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

        audioSource.GetOutputData(spectrumData, 0);

        if (frameIndex % updateEvery == 0)
        {
            spectrumData = new float[4*numTiles];

            audioSource.GetOutputData(spectrumData, 0);

            if(collectSpectralDataForAnalysis)
                FileLog(spectrumData);

            for (int i = 0; i < numTiles; i++)
            {
                float minHeight = -1.5f * screenHalfHeight + tileHeightShow;
                float maxHeight = -1.2f * screenHalfHeight;

                float offsetVal = minHeight + (Mathf.Abs(spectrumData[i + (int)(1.5f*numTiles)]) * (maxHeight - minHeight) * 3);
                offsets[i] = offsetVal;
            }

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

    void FileLog(float[] data)
    {
        TextWriter tw = new StreamWriter("SonicAnalysis/SpectrumData.txt", true);
        tw.WriteLine(String.Join(" ", data));
        tw.Close();
    }
}
