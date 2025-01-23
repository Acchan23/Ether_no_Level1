using System.Collections;
using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.SceneManagement;


namespace Player
{
    public class VictoryCondition : MonoBehaviour
{
    public int requiredEnemies = 20; // Enemigos necesarios para activar el portal
    public float timeLimit = 600f; // Tiempo en segundos (10 minutos)

    public Transform portal; // Portal de salida
    public Transform bossArea; // Área del jefe final
    public Transform player; // El jugador
    public Camera mainCamera; // Cámara principal
    public float cameraMoveDuration = 2f; // Duración del movimiento de cámara
    public Canvas victoryCanvas; // Canvas para victoria
    public Canvas gameOverCanvas; // Canvas para tiempo terminado
    private CameraFollow cameraFollowScript; // Script de seguimiento de la cámara

    // UI con TextMeshPro
    public TextMeshProUGUI enemiesDefeatedText; // Texto para mostrar enemigos derrotados
    public TextMeshProUGUI timeRemainingText; // Texto para mostrar tiempo restante

    private int defeatedEnemies = 0;
    private float elapsedTime = 0f;

    void Start()
    {
        // Desactivar el Canvas de Victory al inicio
        if (victoryCanvas != null)
            victoryCanvas.gameObject.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        UpdateUI(); // Actualiza la interfaz al inicio
    }

    void Update()
    {
        // Incrementar tiempo transcurrido
        elapsedTime += Time.deltaTime;

        // Calcular tiempo restante
        float timeRemaining = Mathf.Max(0, timeLimit - elapsedTime);

        // Actualizar la UI en cada frame
        UpdateUI();

        // Verificar condición de tiempo
        if (timeRemaining <= 0)
        {
            Debug.Log("Tiempo agotado. ¡Perdiste!");

            if (gameOverCanvas != null)
                gameOverCanvas.gameObject.SetActive(true);
            return;
        }

        // Verificar condición de victoria
        if (defeatedEnemies >= requiredEnemies)
        {
            portal.gameObject.SetActive(true); // Activar el portal
            StartCoroutine(MoveCameraToPortal());
            this.enabled = false; // Detener la lógica para no repetir
        }
    }

    public void OnEnemyDefeated()
    {
        defeatedEnemies++;
    }

    private void UpdateUI()
    {
        // Actualizar texto de enemigos derrotados
        if (enemiesDefeatedText != null)
        {
            enemiesDefeatedText.text = $"Enemies Defeated: {defeatedEnemies} / {requiredEnemies}";
        }

        // Actualizar texto de tiempo restante
        if (timeRemainingText != null)
        {
            float timeRemaining = Mathf.Max(0, timeLimit - elapsedTime);
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timeRemainingText.text = $"Time Remaining: {minutes:D2}:{seconds:D2}";
        }
    }

    IEnumerator MoveCameraToPortal()
    {
        if (cameraFollowScript != null)
            cameraFollowScript.enabled = false;

        Vector3 originalPosition = mainCamera.transform.position;
        Quaternion originalRotation = mainCamera.transform.rotation;

        Vector3 targetPosition = bossArea.position + new Vector3(0, 5, -8);
        mainCamera.transform.LookAt(bossArea);

        float elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / cameraMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        mainCamera.transform.position = originalPosition;
        mainCamera.transform.rotation = originalRotation;

        if (cameraFollowScript != null)
            cameraFollowScript.enabled = true;
    }

    public void OnBossDefeated()
    {
        if (victoryCanvas != null)
            victoryCanvas.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    }
}