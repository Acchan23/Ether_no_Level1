using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
       currentHealth = maxHealth;
       player = GameObject.FindGameObjectWithTag("Player").transform;

       // Busca el script VictoryCondition en la escena
       victoryCondition = FindObjectOfType<VictoryCondition>();
    }

    void Die()
    {
       Debug.Log("Enemigo final derrotado.");

       // Notifica a VictoryCondition que el jefe ha sido derrotado
       if (victoryCondition != null)
       {
          victoryCondition.OnBossDefeated();
       }

        Destroy(gameObject); // Destruye al jefe
       }


    private void Update()
    {
        if (currentHealth > maxHealth / 2)
        {
            MeleeAttackMode();
        }
        else
        {
            ChargeAttackMode();
        }
    }

    void MeleeAttackMode()
    {
        // Sigue al jugador para ataque cuerpo a cuerpo
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * movementSpeed * Time.deltaTime;
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

    private void OnCollisionEnter2D(Collision2D collision)
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