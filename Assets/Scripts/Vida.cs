using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("UI")]
    public Slider barraDeVida;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida actual: {vidaActual}");
        if (vidaActual <= 0)
        {
            Muerte();
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

    private void Muerte()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(gameObject);
    }
}