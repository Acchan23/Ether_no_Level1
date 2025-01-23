using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestru : MonoBehaviour
{
     [Header("Settings")]
        public string nombre;

        private Vida vida;
        public GameObject pocima;

        private void Awake()
        {
            vida = GetComponent<Vida>();
        }

        private void Update()
        {
            
            if(vida != null && vida.VidaActual <= 0)
            {
                Debug.Log("El barril ha sido destruido");
                PosibilidadPocima();
                Destroy(gameObject);
            }
        }

         private void PosibilidadPocima ()
        {
            float posibilidad = UnityEngine.Random.Range(0f, 100f);

            if (posibilidad > 50)
            {
                Instantiate(pocima,transform.position,pocima.transform.rotation);
            }
        }
}
