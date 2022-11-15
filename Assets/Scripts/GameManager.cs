using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject boundsPrefab;
    public float colliderThickness = 4f;
    public float zPosition = 0f;
    private Vector2 screenSize;


    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, Transform> boundaries = new Dictionary<string, Transform>();

        boundaries.Add("Top", GameObject.Instantiate(boundsPrefab).transform);
        boundaries.Add("Left", GameObject.Instantiate(boundsPrefab).transform);
        boundaries.Add("Right", GameObject.Instantiate(boundsPrefab).transform);

        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        foreach(KeyValuePair<string, Transform> pair in boundaries)
        {
            pair.Value.name = pair.Key + "Boundary";
            pair.Value.parent = transform;

            if (pair.Key == "Left" || pair.Key == "Right")
                pair.Value.localScale = new Vector3(colliderThickness, screenSize.y * 2, 1);
            else
                pair.Value.localScale = new Vector3(screenSize.x * 2, colliderThickness, 1);
        }

        boundaries["Right"].position = new Vector3(screenSize.x + colliderThickness / 2, 0f, 0f);
        boundaries["Left"].position = -boundaries["Right"].position;
        boundaries["Top"].position = new Vector3(0f, screenSize.y + colliderThickness / 2 + 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
