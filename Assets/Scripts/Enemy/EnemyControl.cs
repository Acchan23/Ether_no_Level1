using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;


namespace Enemy
{
    public class EnemyControl : MonoBehaviour
    {
        [Header("Settings")]
        public float chaseSpeed = 4f;
        public float detectionRange = 5f;
        public float attackRange = 1.5f;
        public float tiempoEntreAtaques = 3f;
        public float dano = 10f;

        private float proximoAtaque;
        private Animator _animator;
        private Rigidbody _rb;
        private bool _isChasing = false;
        private bool _isAttacking = false;
        private bool isDead = false;

        [Header("Player Reference")]
        public Transform player;
        private Vida vida;
        

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            vida = GetComponent<Vida>();
            if (vida == null)
            {
                Debug.LogError("El enemigo no tiene el componente 'Vida'.");
            }

            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            if (player == null)
            {
                Debug.LogError("No se encontró al jugador en la escena. Asegúrate de que tenga la etiqueta 'Player'.");
            }
            
        }


        private void Update()
        {
            if (vida != null && vida.isDead) return;

            if (player == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                _isChasing = false;
                _isAttacking = true;
            }
            else if (distanceToPlayer <= detectionRange)
            {
                _isChasing = true;
                _isAttacking = false;
            }
            else
            {
                _isChasing = false;
                _isAttacking = false;
            }

            if (_isAttacking && Time.time >= proximoAtaque)
            {
                AttackPlayer();
                proximoAtaque = Time.time + tiempoEntreAtaques;
            }
            else if (_isChasing)
            {
                ChasePlayer();
            }

            UpdateAnimation();
        }

        private void ChasePlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            _rb.velocity = direction * chaseSpeed;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * chaseSpeed);
            }
        }

        private void AttackPlayer()
        {
            _rb.velocity = Vector3.zero;

            Vida vidaJugador = player.GetComponent<Vida>();
            if (vidaJugador != null)
            {
                vidaJugador.RecibirDaño(dano);
            }

            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
            }
        }

        public void RecibirDaño(float cantidad)
        {
            if (isDead) return;
            if (vida != null)
            {
                vida.RecibirDaño(cantidad); 
            }
            else
            {
                Debug.LogError("No se encontró el componente Vida en el enemigo.");
            }

            // Implementar lógica de vida
            Debug.Log($"{gameObject.name} recibió {cantidad} de daño.");

        }
           

        private void UpdateAnimation()
        {
            float speed = _rb.velocity.magnitude;

            if (_animator != null)
            {
                _animator.SetFloat("Speed", speed);
                _animator.SetBool("IsChasing", _isChasing);
                _animator.SetBool("IsAttacking", _isAttacking);
            }
        }
    }
}