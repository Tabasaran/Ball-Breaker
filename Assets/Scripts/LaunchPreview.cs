using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPreview : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField]
    private float lineLenth = 10f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void SetStartPoint(Vector3 worldPoint)
    {
        lineRenderer.SetPosition(0, worldPoint);
        
    }

    public void SetEndPoint(Vector3 worldPoint)
    {
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0) + worldPoint * lineLenth);
        
        if (lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
        }
    }
    public void Hide()
    {
        lineRenderer.enabled = false;
    }
}
