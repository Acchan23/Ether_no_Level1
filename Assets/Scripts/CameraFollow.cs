using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // The player or target to follow
    public Vector3 offset = new Vector3(0, 15, -5); // Offset of the camera relative to the target
    public float smoothSpeed = 0.125f; // Speed of the camera's smooth movement

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned for the camera to follow!");
            return;
        }

        // Calculate the desired position
        Vector3 desiredPosition = target.position + Quaternion.Euler(45, 0, 0) * offset;

        // Smooth the camera movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the position to the camera
        transform.position = smoothedPosition;

        // Optional: Rotate the camera to look at the target
        transform.LookAt(target);
    }
}
