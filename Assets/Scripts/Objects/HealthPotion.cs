using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
   public int heal = 20;

   private void OnTriggerEnter(Collider player)
   {
    if (player.CompareTag("Player"))
    {
        Vida vida = player.GetComponent<Vida>();
        if(vida != null)
        {
            vida.Curar(heal);
            Destroy(gameObject);
            Debug.Log("Has sumado 20 de vida");
        }
    }
    
    
   }
}
