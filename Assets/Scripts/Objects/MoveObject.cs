using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
   public float moveSpeed = 2f; // Velocidad de movimiento
    public float moveDistance = 3f; // Distancia máxima que recorrerá

    private Vector3 startPosition;

    void Start()
    {
        // Guarda la posición inicial de la plataforma
        startPosition = transform.position;
    }

    void Update()
    {
        
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
