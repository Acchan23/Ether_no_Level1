using System.Collections;
using Player;
using UnityEngine;

namespace Enemy
{
    public class FinalBoss : MonoBehaviour
    {
        public float meleeAttackDamage = 20f;
        public float chargeAttackDamage = 30f;

        public float movementSpeed = 2f;
        public float chargeSpeed = 5f;

        private Vector3 battleAreaCenter;
        public float battleAreaRadius = 10f;

        private Transform player;
        private bool isCharging = false;
        private Animator animator;

        // Parámetros de fases
        private int currentPhase = 1;
        public float phaseChangeTimer = 10f; // Tiempo entre fases
        private float phaseTimer;

        public VictoryCondition victoryCondition;

        private void Start()
        {
            // Inicialización de referencias
            victoryCondition = FindObjectOfType<VictoryCondition>();
            if (victoryCondition == null)
            {
                Debug.LogError("VictoryCondition no encontrado. Por favor, asegúrate de que está en la escena.");
            }
            player = GameObject.FindGameObjectWithTag("Player").transform;
            animator = GetComponent<Animator>();

            battleAreaCenter = new Vector3(92, 7.41f, 58);
            phaseTimer = phaseChangeTimer;

            Debug.Log($"Área de batalla: Centro = {battleAreaCenter}, Radio = {battleAreaRadius}");
        }

        private void Update()
        {
            // Actualizar lógica según fase
            HandlePhase();

            // Verificar si el jefe está fuera del área de batalla
            if (Vector3.Distance(transform.position, battleAreaCenter) > battleAreaRadius)
            {
                Debug.Log("Boss fuera del área, regresando.");
                ReturnToBattleArea();
                return;
            }
        }

        private void HandlePhase()
        {
            phaseTimer -= Time.deltaTime;

            if (phaseTimer <= 0)
            {
                currentPhase++;
                phaseTimer = phaseChangeTimer; // Reiniciar el temporizador
                Debug.Log($"Cambiando a la fase {currentPhase}");
            }

            switch (currentPhase)
            {
                case 1:
                    MeleeAttackMode();
                    break;
                case 2:
                    ChargeAttackMode();
                    break;
                default:
                    MeleeAttackMode(); // Fase por defecto
                    break;
            }
        }

        private void ReturnToBattleArea()
        {
            Vector3 directionToCenter = (battleAreaCenter - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, battleAreaCenter, movementSpeed * Time.deltaTime);
        }

        private void MeleeAttackMode()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= battleAreaRadius)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * movementSpeed * Time.deltaTime;

                if (animator != null)
                {
                    animator.SetTrigger("MeleeAttack");
                }
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

            if (player != null)
            {
                if (animator != null)
                {
                    animator.SetTrigger("ChargeAttack");
                }

                Vector3 direction = (player.position - transform.position).normalized;
                float chargeDuration = 0.5f;
                float elapsedTime = 0f;

                while (elapsedTime < chargeDuration)
                {
                    Vector3 newPosition = transform.position + direction * chargeSpeed * Time.deltaTime;

                    if (Vector3.Distance(newPosition, battleAreaCenter) <= battleAreaRadius)
                    {
                        transform.position = newPosition;
                    }
                    else
                    {
                        break;
                    }

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            yield return new WaitForSeconds(2f);
            isCharging = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Aplicar daño al jugador según la fase actual
                float damage = currentPhase == 2 ? chargeAttackDamage : meleeAttackDamage;
                collision.gameObject.GetComponent<Vida>()?.RecibirDaño(damage);
            }
        }

        private void OnDestroy()
        {
            Debug.Log("El jefe ha sido derrotado.");

            if (victoryCondition != null)
            {
                Debug.Log("Llamando a VictoryCondition.OnBossDefeated.");
                victoryCondition.OnBossDefeated();
            }
            else
            {
                Debug.LogError("VictoryCondition no está configurado o no existe.");
            }
        }
    }
}
