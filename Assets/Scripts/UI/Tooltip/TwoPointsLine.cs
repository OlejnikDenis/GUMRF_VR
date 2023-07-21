using UnityEngine;

[ExecuteInEditMode]
public class TwoPointsLine : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private LineRenderer _lineRenderer;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    void Update()
    {
        _lineRenderer.SetPosition(0, pointA.position);
        _lineRenderer.SetPosition(1, pointB.position);
    }
}
