using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public Transform cameraTransform; // Asigna la cámara principal en el inspector
    public float shakeDuration = 0.5f; // Duración de la vibración
    public float shakeMagnitude = 0.1f; // Intensidad de la vibración
    public CameraFollow cameraFollow;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Asignar la cámara principal automáticamente
        }
        originalPosition = cameraTransform.localPosition;
    }

    public void StartShake()
    {
        StopAllCoroutines(); // Por si hay otra vibración en curso
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        cameraFollow.isShaking = true; // Desactiva el seguimiento
        float elapsed = 0f;

        Vector3 originalPosition = cameraFollow.target.position + cameraFollow.offset;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            cameraTransform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = originalPosition;
        cameraFollow.isShaking = false; // Reactiva el seguimiento
    }

}
