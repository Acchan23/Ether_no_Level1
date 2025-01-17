using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
    [Header("Configuración del personaje")]
    public float vidaMaxima = 1000f;
    private float vidaActual;

    [Header("UI")]
    public Slider barraDeVida; // Referencia a la barra de vida en la UI
    public Text textoVida;     // Texto para mostrar la cantidad de vida

    private void Start()
    {
        // Configuración inicial
        vidaActual = vidaMaxima;

        // Configurar la barra de vida
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }

        ActualizarTextoVida();
    }

    // Aumentar la vida al recoger un ítem
    public void AumentarVida(float cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima); // Evita que la vida supere el máximo
        Debug.Log($"Vida aumentada en {cantidad}. Vida actual: {vidaActual}");

        // Actualizar UI
        if (barraDeVida != null) barraDeVida.value = vidaActual;
        ActualizarTextoVida();
    }

    // Método para reducir la vida por un ataque
    public void ReducirVida(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima); // Evita valores negativos
        Debug.Log($"Vida reducida en {cantidad}. Vida actual: {vidaActual}");

        if (vidaActual <= 0)
        {
            Muerte();
        }

        // Actualizar UI
        if (barraDeVida != null) barraDeVida.value = vidaActual;
        ActualizarTextoVida();
    }

    // Método para mostrar la cantidad de vida
    private void ActualizarTextoVida()
        {
        if (textoVida != null)
            {
            textoVida.text = $"Vida: {vidaActual}/{vidaMaxima}";
            }
        }

    // Método llamado cuando la vida llega a 0
    private void Muerte()
        {
        Debug.Log("El personaje ha muerto.");
        // Agregar lógica de muerte (devolver al checkpoint o al inicio) y animación. Falta
        }
    }
}


