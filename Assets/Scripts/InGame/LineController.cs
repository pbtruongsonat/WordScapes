using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector3> listPosition;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    public void AddPoint(Vector3 position)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1, position);
    }

    public void RemovePoint()
    {
        lineRenderer.positionCount = 0;
    }
}
