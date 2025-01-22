using System.Collections;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // El prefab del enemigo
    public int numberOfEnemies = 20; // Número total de enemigos a instanciar
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
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}


