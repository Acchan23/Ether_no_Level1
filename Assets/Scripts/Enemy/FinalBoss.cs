using System.Collections;
using UnityEngine;
using Player;

namespace Enemy
{
    public class FinalBoss : MonoBehaviour
    {
        public float maxHealth = 100f;
        private float currentHealth;

        public float meleeAttackDamage = 20f;
        public float chargeAttackDamage = 30f;

        public float movementSpeed = 2f;
        public float chargeSpeed = 5f;

        private Transform player;
        private bool isCharging = false;
        private VictoryCondition victoryCondition;
        private Animator animator; // Referencia al Animator

        private void Start()
        {
            currentHealth = maxHealth;
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // Busca el script VictoryCondition en la escena
            victoryCondition = FindObjectOfType<VictoryCondition>();

            // Obtén el componente Animator
            animator = GetComponent<Animator>();
        }

        void Die()
        {
            Debug.Log("Enemigo final derrotado.");

            // Notifica a VictoryCondition que el jefe ha sido derrotado
            if (victoryCondition != null)
            {
                victoryCondition.OnBossDefeated();
            }

            Destroy(gameObject, 2f);
        }

        private void Update()
        {
            if (currentHealth > maxHealth / 2)
            {
                //animator.SetBool("IsIdle", false); // Asegúrate de que Idle no esté activo
                MeleeAttackMode();
            }
            else
            {
                //animator.SetBool("IsIdle", false);
                ChargeAttackMode();
            }
        }

        void MeleeAttackMode()
        {
            if (player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * movementSpeed * Time.deltaTime;

                animator.SetTrigger("MeleeAttack"); // Activa la animación de ataque cuerpo a cuerpo
            }
        }

        void ChargeAttackMode()
        {
            if (!isCharging)
            {
                StartCoroutine(Charge());
            }
        }

        System.Collections.IEnumerator Charge()
        {
            isCharging = true;

            // Embestida
            if (player != null)
            {
                animator.SetTrigger("ChargeAttack"); // Activa la animación de embestida

                Vector3 direction = (player.position - transform.position).normalized;
                float chargeDuration = 0.5f; // Ajustar duración según necesidad
                float elapsedTime = 0f;

                while (elapsedTime < chargeDuration)
                {
                    transform.position += direction * chargeSpeed * Time.deltaTime;
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            // Reposo después de la embestida
            yield return new WaitForSeconds(2f);

            isCharging = false;
        }

        public void RecibirDaño(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (currentHealth > maxHealth / 2)
                {
                    // Daño de ataque cuerpo a cuerpo
                    collision.gameObject.GetComponent<Vida>().RecibirDaño(meleeAttackDamage);
                }
                else
                {
                    // Daño de embestida
                    collision.gameObject.GetComponent<Vida>().RecibirDaño(chargeAttackDamage);
                }
            }
        }
    }
}
