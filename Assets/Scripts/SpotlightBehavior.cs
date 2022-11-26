using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehavior : MonoBehaviour
{

    [Range(0f, 1f)]
    public float opacity = 0.5f;
    private float lerpFactor;

    private LineRenderer lineRenderer;


    private Vector3 beamTarget;
    private Vector2 screenSize;

    private Vector3 beamCurrentPosition;

    private float eps = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        beamTarget = GenerateBeamTarget();
        beamCurrentPosition = beamTarget;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, beamCurrentPosition);
        lineRenderer.material.SetColor("_Color", new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), opacity));

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(beamCurrentPosition, beamTarget) < eps)
        {
            beamTarget = GenerateBeamTarget();
            lerpFactor = Random.Range(0.01f, 0.1f);
            lineRenderer.material.SetColor("_Color", new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), opacity));
        }

        beamCurrentPosition = Vector3.Lerp(beamCurrentPosition, beamTarget, lerpFactor);
        lineRenderer.SetPosition(1, beamCurrentPosition);
    }

    private Vector3 GenerateBeamTarget()
    {
        Vector2 pointOnCircle = Random.insideUnitCircle.normalized * 2 * screenSize.x;
        return transform.position + new Vector3(pointOnCircle.x, -Mathf.Abs(pointOnCircle.y) - 1f, 0f);
    }
}
