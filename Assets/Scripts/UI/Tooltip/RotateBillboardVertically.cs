using UnityEngine;

public class RotateBillboardVertically : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;

    private void Start()
    {
        if (targetCamera) return;
        
        targetCamera = Camera.main.transform;
    }
    
    void Update()
    {
        // var targerDirection = targetCamera.position - transform.position;
        //
        // var singleStep = 1 * Time.deltaTime;
        //
        // var newDirection = Vector3.RotateTowards(transform.forward, targerDirection, singleStep, 0.0f);
        //
        // Debug.DrawRay(transform.position, newDirection, Color.red);
        //
        // transform.rotation = Quaternion.LookRotation(newDirection);
        
        transform.LookAt(targetCamera, Vector3.up);
    }
}
