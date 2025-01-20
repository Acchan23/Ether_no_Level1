using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        private Animator _animator;      // Referencia al Animator
        private PlayerMovement _playerMovement; // Referencia al script PlayerMovement
        private PlayerAttack _playerAttack;     // Referencia al script PlayerAttack
        // private Rigidbody _rb; // Eliminar esta línea si no es necesaria


        void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();
            //_rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Obtenemos la magnitud del vector de movimiento
            float movementMagnitude = _playerMovement.GetMovementMagnitude();

            // Establecemos los parámetros de movimiento en el Animator
            _animator.SetFloat("Speed", movementMagnitude);

            // Verificamos si el jugador está corriendo
            bool isRunning = Input.GetKey(KeyCode.LeftShift) && movementMagnitude > 0;
            _animator.SetBool("IsRunning", isRunning);

            // Verificamos si el jugador está en el suelo
            bool isGrounded = _playerMovement.IsGrounded();
            _animator.SetBool("IsGrounded", isGrounded);

            // Verificamos si el jugador está saltando
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                _animator.SetTrigger("Jump");
            }

            // Verificamos si el jugador ataca
            if (Input.GetMouseButtonDown(0)) // Click izquierdo del mouse
            {
                _animator.SetTrigger("Attack");
                _playerAttack.Atacar(); // Ejecutamos la lógica de ataque
            }
        }
    }
}
