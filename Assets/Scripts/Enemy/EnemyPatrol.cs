using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;                  // Punto A para patrullar
    public Transform pointB;                  // Punto B para patrullar

    [Header("Settings")]
    public float patrolSpeed = 2f;           // Velocidad mientras patrulla
    public float chaseSpeed = 4f;            // Velocidad cuando persigue al jugador
    public float detectionRange = 5f;        // Rango de detección del jugador
    private float proximoAtaque;
    private float tiempoEntreAtaques = 2f;

    [Header("Player Reference")]
    public Transform player;                 // Referencia al transform del jugador

    private Transform _currentTarget;        
    private Rigidbody _rb;                   
    private Animator _animator;              
    private bool _isChasing;                 
    public float attackRange = 1.5f; 
    private bool _isAttacking = false; 
    public float dano = 10f;


    void Awake()
    {
        // Inicializamos componentes y estado
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _currentTarget = pointA;             // Comenzamos en el punto A
        _isChasing = false;
    }

    void Update()
    {
        // Calcular distancia al jugador
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

        // Ejecutar comportamiento basado en el estado
        if (_isAttacking)
        {
            AttackPlayer();
            proximoAtaque = Time.time + tiempoEntreAtaques;
        }
        else if (_isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Actualizar parámetros del Animator
        UpdateAnimation();
    }

    private void Patrol()
    {
        // Mover hacia el objetivo puntos A y B
        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        direction.y = 0; 
        _rb.velocity = direction * patrolSpeed;

        // Cambiar la orientación hacia el objetivo actual
    if (direction != Vector3.zero)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * patrolSpeed);
    }

        // Cambiar objetivo cuando se alcanza el punto actual
        if (Vector3.Distance(transform.position, _currentTarget.position) < 0.1f)
        {
            _currentTarget = _currentTarget == pointA ? pointB : pointA;
        }
    }

    private void ChasePlayer()
    {
        // Mover hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Bloquear movimiento en Y
        _rb.velocity = direction * chaseSpeed;

        if (direction != Vector3.zero)
        {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * chaseSpeed);
        }
    }

    private void AttackPlayer()
    {
        // Detener movimiento
        _rb.velocity = Vector3.zero;

        // Orientar hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * patrolSpeed);
        }

        Vida vidaJugador = player.GetComponent<Vida>();
        if (vidaJugador != null)
        {
            vidaJugador.RecibirDaño(dano);
        }

        Debug.Log($"{gameObject.name} atacó al jugador");

        // Realizar ataque (en la animación puede haber un evento que aplique el daño)
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void UpdateAnimation()
    {
        // Calcular velocidad actual
        float speed = _rb.velocity.magnitude;

        // Actualizar los parámetros del Animator
        _animator.SetFloat("Speed", speed);               // Velocidad del enemigo
        _animator.SetBool("IsChasing", _isChasing);       // Estado de persecución
        _animator.SetBool("IsAttacking", _isAttacking);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de detección para depuración
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
