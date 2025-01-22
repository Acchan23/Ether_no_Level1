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
        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Botón izquierdo del mouse para atacar
            {
                StartCoroutine(Atacar());
            }
        }

        public IEnumerator Atacar()
        {
            _animator.SetBool("IsAttacking", true);

            // Detectar enemigos
            Collider[] enemigos = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);
            foreach (Collider enemigo in enemigos)
            {
                Vida vidaEnemigo = enemigo.GetComponent<Vida>();
                if (vidaEnemigo != null)
                {
                    vidaEnemigo.RecibirDaño(daño);
                }
            }

            yield return new WaitForSeconds(0.5f); // Duración de la animación de ataque
            _animator.SetBool("IsAttacking", false);
        }
    }
}
