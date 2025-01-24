using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Player;

public class Vida : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaMaxima = 100f;
    private float vidaActual;
    private Animator animator;
    public bool isDead;

    [Header("Referencias")]
    public CameraEffects cameraEffects;
    public Slider barraDeVida;
    public Canvas muerteCanvas;
    private VictoryCondition victoryCondition;  // Para manejar condiciones de victoria

    public event System.Action OnRecibirDaño;
    public event System.Action OnMuerte;

    public float VidaActual => vidaActual;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // Buscar el VictoryCondition solo si no está asignado
        if (victoryCondition == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                victoryCondition = jugador.GetComponent<VictoryCondition>();
            }
        }

        ValidarReferencias();
    }

    private void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarraDeVida();
        muerteCanvas?.gameObject.SetActive(false);
    }

    public void RecibirDaño(float cantidad)
    {
        if (isDead || cantidad <= 0) return;

        vidaActual = Mathf.Clamp(vidaActual - cantidad, 0, vidaMaxima);
        ActualizarBarraDeVida();
        cameraEffects?.StartShake();
        OnRecibirDaño?.Invoke();

        if (vidaActual <= 0 && !isDead)
        {
            StartCoroutine(Muerte());
        }
    }

    public void Curar(float cantidad)
    {
        if (isDead || cantidad <= 0) return;

        vidaActual = Mathf.Clamp(vidaActual + cantidad, 0, vidaMaxima);
        ActualizarBarraDeVida();
    }

    private IEnumerator Muerte()
    {
        isDead = true;
        OnMuerte?.Invoke();

        animator?.SetBool("IsDead", true);
        animator?.Play("Die");

        // Verificar si es un enemigo o jefe
        if (CompareTag("Enemy") && victoryCondition != null)
        {
            victoryCondition.OnEnemyDefeated();
        }
        else if (CompareTag("Boss") && victoryCondition != null)
        {
            victoryCondition.OnBossDefeated();
        }

        // Desactivar el control del jugador si aplica
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        yield return new WaitForSeconds(3f);

        if (muerteCanvas != null)
        {
            muerteCanvas.gameObject.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    private void ActualizarBarraDeVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }
    }

    private void ValidarReferencias()
    {
        if (cameraEffects == null) Debug.LogWarning("Falta asignar CameraEffects.");
        if (barraDeVida == null) Debug.LogWarning("Falta asignar BarraDeVida.");
        if (muerteCanvas == null) Debug.LogWarning("Falta asignar MuerteCanvas.");
        if (victoryCondition == null) Debug.LogWarning("Falta asignar VictoryCondition.");
    }
}
