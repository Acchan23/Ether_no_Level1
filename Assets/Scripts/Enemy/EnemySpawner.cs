using System.Collections;
using UnityEngine;


namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int numberOfEnemies = 20;
    public float spawnInterval = 2f; // Intervalo de tiempo entre cada spawn
    public Vector3 spawnAreaMin; // Coordenadas mínimas del área de spawn
    public Vector3 spawnAreaMax; // Coordenadas máximas del área de spawn

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

        private IEnumerator SpawnEnemies()
        {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Esperar antes de spawnear el siguiente enemigo
        }
        }

        private void SpawnEnemy()
        {
        // Generar una posición aleatoria dentro del área de spawn
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

        // Instanciar el enemigo en la posición generada
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Asegurarse de que los enemigos generados tengan las referencias necesarias configuradas
        EnemyControl enemyControl = newEnemy.GetComponent<EnemyControl>();
        if (enemyControl != null)
        {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            {
            enemyControl.player = playerObject.transform;
            }
            else
            {
            Debug.LogError("No se encontró al jugador con la etiqueta 'Player' para asignar al enemigo.");
            }
        }
}


        private void OnDrawGizmos()
        {
        // Dibuja un cubo que representa el área de spawn
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Color rojo con transparencia
        Vector3 size = spawnAreaMax - spawnAreaMin;
        Vector3 center = spawnAreaMin + size / 2;
        Gizmos.DrawCube(center, size);
        }

    }

}

