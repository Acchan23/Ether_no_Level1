using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;                // Velocidad normal de movimiento
        public float runningSpeed = 8f;        // Velocidad al correr
        public float rotationSpeed = 720f;     // Velocidad de rotación
        public float jumpForce = 6f;           // Fuerza de salto
        public float groundCheckDistance = 0.5f; // Distancia para comprobar el suelo
        private Rigidbody _rb;                 // Referencia al Rigidbody
        private Vector3 _movement;             // Vector de movimiento
        private bool _isGrounded;              // Si el jugador está en el suelo

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Creamos el vector de movimiento basado en las entradas
            _movement = new Vector3(horizontal, 0, vertical).normalized;

            // Verificamos si el jugador está tocando el suelo
            CheckGrounded();

            // Saltar solo si está en el suelo
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void FixedUpdate()
        {
            // Determinar si el jugador está corriendo
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runningSpeed : speed;

            // Aplicamos movimiento
            Vector3 move = currentSpeed * Time.fixedDeltaTime * _movement;
            _rb.MovePosition(_rb.position + move);

            // Rotación hacia la dirección del movimiento (si hay movimiento)
            if (_movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_movement);
                _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }

        private void CheckGrounded()
        {
            // Lanza un rayo hacia abajo para verificar si está tocando el suelo
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Opcional: Log para debug de colisiones
            Debug.Log("Colisión con: " + collision.gameObject.name);
        }

        private void OnTriggerEnter(Collider player)
        {
            if (player.CompareTag("TeleportTrigger"))
            {
                Teleport();
            }
        }

        private void Teleport()
        {
            transform.position = new Vector3(10, 9.5f, 29);
            Debug.Log("Teleportado");
        }

        public Vector3 GetMovement()
        {
            return _movement; // Devolvemos el vector de movimiento actual
        }

        public float GetMovementMagnitude()
        {
            return _movement.magnitude; // Magnitud del movimiento
        }

        public bool IsGrounded()
        {
            return _isGrounded; // Estado del suelo
        }
    }
   

}
