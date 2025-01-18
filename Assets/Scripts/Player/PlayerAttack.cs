using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
  
    public float daño = 25f;
    public Transform puntoDeAtaque;
    public float rangoDeAtaque = 2f;
    public LayerMask capaEnemigos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Botón de ataque
        {
            Atacar();
        }
    }

    private void Atacar()
    {
        // Detectar enemigos dentro del rango
        Collider[] enemigos = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigos)
        {
            Vida vidaEnemigo = enemigo.GetComponent<Vida>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDaño(daño);
            }
        }

        Debug.Log("Jugador atacó");
        }

    private void OnDrawGizmosSelected()
        {
        // Dibujar el rango del ataque en el editor para fines de depuración
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque);
        }
        }
    }   
}

