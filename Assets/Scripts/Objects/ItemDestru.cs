using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestru : MonoBehaviour
{
     [Header("Settings")]
        public string nombre;

        public int vida;
        public GameObject pocima;

        private void Update()
        {
            
            if(vida <= 0)
            {
                PosibilidadMoneda();
                Destroy(gameObject);
            }
        }

         private void PosibilidadMoneda ()
        {
            float posibilidad = UnityEngine.Random.Range(0f, 100f);

            if (posibilidad > 50)
            {
                Instantiate(pocima,transform.position,pocima.transform.rotation);
            }
        }
}
