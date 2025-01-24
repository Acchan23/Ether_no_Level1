using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // The player or target to follow
    public Vector3 offset = new Vector3(0, 5, -5); // Offset of the camera relative to the target
    public float smoothSpeed = 0.135f; 

     void Start()
    {
       
        transform.position = target.position + offset;
        
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    private void LateUpdate()
    {
          if (target != null)
        {
            transform.position = target.position + offset;
        }
        
        if (target == null)
        {
            Debug.LogWarning("No target assigned for the camera to follow!");
            return;
        }

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }

}

