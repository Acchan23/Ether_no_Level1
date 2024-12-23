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

    [Header("Player Reference")]
    public Transform player;                 // Referencia al transform del jugador

    private Transform _currentTarget;        // Objetivo actual de patrulla
    private Rigidbody _rb;                   // Rigidbody del enemigo
    private bool _isChasing;                 // Estado: persiguiendo o no

    void Awake()
    {
        // Inicializamos componentes y estado
        _rb = GetComponent<Rigidbody>();
        _currentTarget = pointA;             // Comenzamos en el punto A
        _isChasing = false;
    }

    void Update()
    {
        // Calcular distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Determinar estado: patrulla o persigue al jugador
        _isChasing = distanceToPlayer <= detectionRange;

        // Cambiar comportamiento basado en el estado
        if (_isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Mover hacia el objetivo actual (puntos A y B)
        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        _rb.velocity = direction * patrolSpeed;

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
        _rb.velocity = direction * chaseSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de detección para depuración
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
