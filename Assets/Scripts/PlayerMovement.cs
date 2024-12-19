using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;           // Velocidad de movimiento
    public float rotationSpeed = 720f; // Velocidad de rotación (grados por segundo)
    private Rigidbody _rb;            // Referencia al Rigidbody
    private Vector3 _movement;        // Vector de dirección del movimiento

    void Awake()
    {
        // Obtenemos el componente Rigidbody
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Capturamos la entrada del jugador (Horizontal y Vertical)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Creamos el vector de movimiento basado en las entradas
        _movement = new Vector3(horizontal, 0, vertical).normalized;
    }

    void FixedUpdate()
    {
        // Aplicamos movimiento
        Vector3 move = _movement * speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + move);

        // Rotación hacia la dirección del movimiento (si hay movimiento)
        if (_movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movement);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportTrigger"))
        {
            Teleport();
        }
    }
    void Teleport()
    {
        transform.position = new Vector3(-2,1,6);
        Debug.Log ("Teleportado");
    }
}
