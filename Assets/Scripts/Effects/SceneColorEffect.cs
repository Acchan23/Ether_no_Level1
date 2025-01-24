using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneColorEffect : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Color Settings")]
    public Color damageColor = Color.red; // Color al recibir daño
    public float colorDuration = 0.5f;

    private Color originalColor;

    private void Start()
    {
        mainCamera = Camera.main;
        originalColor = mainCamera.backgroundColor; // Guarda el color original
    }

    public void ChangeSceneColor()
    {
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        mainCamera.backgroundColor = damageColor; // Cambia al color de daño
        yield return new WaitForSeconds(colorDuration); // Espera
        mainCamera.backgroundColor = originalColor; // Vuelve al color original
    }
}
