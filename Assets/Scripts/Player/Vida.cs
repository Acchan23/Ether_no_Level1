using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Enemy;
using Player;

public class Vida : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;
    private Animator animator;
    public bool isDead = false;
    private VictoryCondition victoryCondition;
    public CameraEffects cameraEffects;
    


    public float VidaActual
    {
        get { return vidaActual; }
    }

    [Header("UI")]
    public Slider barraDeVida;

    [Header("Canvas")]
    public Canvas muerteCanvas; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            victoryCondition = jugador.GetComponent<VictoryCondition>();
        }
    }

    private void Start()
    {
        vidaActual = vidaMaxima;

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima; // Configura el valor máximo de la barra
            barraDeVida.value = vidaActual;   // Ajusta el valor inicial de la barra
        }

        if (muerteCanvas != null)
        {
            muerteCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy") && !isDead) 
    {
        EnemyControl enemy = other.GetComponent<EnemyControl>();
        if (enemy != null)
        {
            enemy.RecibirDaño(20f); 
        }
    }
}

    public void RecibirDaño(float cantidad)
    {
        Debug.Log($"{gameObject.name} antes de recibir daño: Vida actual: {vidaActual}");

        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima); // Limitar entre 0 y vida máxima

        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida actual: {vidaActual}");

        // Actualizar barra de vida
        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }

        // Efectos de daño
        if (cameraEffects != null)
        {
            cameraEffects.StartShake(); // Llamar a la vibración
        }

       

        // Verificar si el jugador muere
        if (vidaActual <= 0 && !isDead)
        {
            StartCoroutine(Muerte());
        }
    }


    public void Curar(float cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Clamp(vidaActual + cantidad, 0, vidaMaxima);
        Debug.Log($"{gameObject.name} se curó {cantidad}. Vida actual: {vidaActual}");

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }
    }

   private IEnumerator Muerte()
    {
        if (isDead)
    {
        yield break;
    }
        
        isDead = true;
        Debug.Log($"{gameObject.name} ha muerto.");
    
        if (animator != null)
        {
        animator.SetBool("IsDead", true); // Activa la animación de muerte
        animator.Play("Die");         // Forzar la animación de muerte
        }

        if (CompareTag("Enemy") && victoryCondition != null)
        {
            victoryCondition.OnEnemyDefeated();
        }

        // Desactivar control del jugador
        var playerControl = GetComponent<PlayerMovement>();
        if (playerControl != null)
            playerControl.enabled = false;

        yield return new WaitForSeconds(3f); // Tiempo para mostrar animación

        if (muerteCanvas != null)
        {
        muerteCanvas.gameObject.SetActive(true); // Muestra el Canvas de "Has muerto"
        }
        Destroy(gameObject);  
    }


    public void ReiniciarNivel()
    {
    if (muerteCanvas != null)
        muerteCanvas.gameObject.SetActive(false);

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
