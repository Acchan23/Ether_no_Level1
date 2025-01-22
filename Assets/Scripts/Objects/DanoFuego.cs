using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoFuego : MonoBehaviour
{

    public float danoPorSegundo = 10f;

    private void OnTriggerStay(Collider player)
    {
        // Verifica si el objeto que colisiona tiene el script "Vida"
        Vida vida = player.GetComponent<Vida>();
        if (vida != null)
        {
            // Aplica daño basado en el tiempo que permanece dentro del fuego
            vida.RecibirDaño(danoPorSegundo * Time.deltaTime);
            print(danoPorSegundo);
        }
    }
}