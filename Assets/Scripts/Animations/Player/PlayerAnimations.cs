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
        private bool isAttacking;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();
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
                _animator.SetBool("IsJumping", true);
                StartCoroutine(ResetJumpParameter());
            }

            // Verificamos si el jugador ataca
            if (Input.GetMouseButtonDown(0)) // Click izquierdo del mouse
            {
                _animator.SetBool("IsAttacking", isAttacking);
                _playerAttack.Atacar(); // Ejecutamos la lógica de ataque
            }
        }

        // Corutina para restablecer el parámetro IsJumping después del salto
        private IEnumerator ResetJumpParameter()
        {
            yield return new WaitForSeconds(0.1f); // Ajusta el tiempo según la duración del salto
            _animator.SetBool("IsJumping", false);
        }
    }
}
