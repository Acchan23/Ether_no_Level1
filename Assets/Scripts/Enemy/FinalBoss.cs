using System.Collections;
using UnityEngine;
using Player;

namespace Enemy
{
    public class FinalBoss : MonoBehaviour
    {
        // Salud del jefe
        public float maxHealth = 100f;
        private float currentHealth;

        // Daño de ataques
        public float meleeAttackDamage = 20f;
        public float chargeAttackDamage = 30f;

        // Velocidad de movimiento
        public float movementSpeed = 2f;
        public float chargeSpeed = 5f;

        // Área de batalla
        private Vector3 battleAreaCenter; // Centro del área de batalla
        public float battleAreaRadius = 10f; // Radio del área de batalla

        // Referencias
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

            // Asignar el área de batalla a la posición fija del barco
            battleAreaCenter = new Vector3(92, 7.40999985f, 58);

            Debug.Log($"Área de batalla establecida: Centro = {battleAreaCenter}, Radio = {battleAreaRadius}");
        }

        private void Update()
        {
            // Verificar si el jefe está dentro del área de batalla
            if (Vector3.Distance(transform.position, battleAreaCenter) > battleAreaRadius)
            {
                Debug.Log("Boss fuera del área de batalla, reposicionándolo.");
                ReturnToBattleArea();
                return; // Detener cualquier otra lógica de movimiento hasta regresar al área
            }

            // Comportamiento normal del jefe
            if (currentHealth > maxHealth / 2)
            {
                MeleeAttackMode();
            }
            else
            {
                ChargeAttackMode();
            }
        }

        void ReturnToBattleArea()
        {
            // Mover al jefe hacia el centro del área de batalla
            Vector3 directionToCenter = (battleAreaCenter - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, battleAreaCenter, movementSpeed * Time.deltaTime);

            // Asegurar que el jefe no termine fuera del área
            if (Vector3.Distance(transform.position, battleAreaCenter) < 0.5f) // Tolerancia de 0.5 unidades
            {
                transform.position = battleAreaCenter;
            }
        }

        private void MeleeAttackMode()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= battleAreaRadius)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * movementSpeed * Time.deltaTime;

                animator.SetTrigger("MeleeAttack"); // Activa la animación de ataque cuerpo a cuerpo
            }
        }

        private void ChargeAttackMode()
        {
            if (!isCharging)
            {
                StartCoroutine(Charge());
            }
        }

        private IEnumerator Charge()
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
                    Vector3 newPosition = transform.position + direction * chargeSpeed * Time.deltaTime;

                    // Verificar si la nueva posición está dentro del área de batalla
                    if (Vector3.Distance(newPosition, battleAreaCenter) <= battleAreaRadius)
                    {
                        transform.position = newPosition;
                    }
                    else
                    {
                        break; // Detener el movimiento si se sale del área
                    }

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

        private void Die()
        {
            Debug.Log("Enemigo final derrotado.");

            // Notifica a VictoryCondition que el jefe ha sido derrotado
            if (victoryCondition != null)
            {
                victoryCondition.OnBossDefeated();
            }

            Destroy(gameObject, 2f);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentHealth > maxHealth / 2)
                {
                    // Daño de ataque cuerpo a cuerpo
                    other.GetComponent<Vida>().RecibirDaño(meleeAttackDamage);
                }
                else
                {
                    // Daño de embestida
                    other.GetComponent<Vida>().RecibirDaño(chargeAttackDamage);
                }
            }
        }
    }
}
