using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Vida : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;
    private Animator animator;
    public bool isDead = false;

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
            muerteCanvas.gameObject.SetActive(false); // Asegúrate de que el Canvas esté desactivado al inicio
        }
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima); // Limitar entre 0 y vida máxima

        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida actual: {vidaActual}");

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual; // Actualiza la barra de vida
        }

        if (vidaActual <= 0)
        {
            StartCoroutine(Muerte()); // Llamar corrutina
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
        isDead = true;
        Debug.Log($"{gameObject.name} ha muerto.");
        animator.SetBool("IsDead", true); // Activa la animación de muerte
        animator.Play("Muerte"); // Forzar la animación de muerte

        yield return new WaitForSeconds(4f); // Espera antes de mostrar el Canvas

        if (muerteCanvas != null)
        {
            muerteCanvas.gameObject.SetActive(true); // Activa el Canvas de "Has muerto"
        }
    }

 
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia el nivel actual
    }
}
