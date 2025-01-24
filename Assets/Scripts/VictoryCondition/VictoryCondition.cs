using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Player
{
    public class VictoryCondition : MonoBehaviour
    {
        public int requiredEnemies = 20;
        public float timeLimit = 600f;

        public Transform portal;
        public Transform bossArea;
        public Transform player;
        public Transform boss;
        public Camera mainCamera;
        public float cameraMoveDuration = 2f;
        public Canvas victoryCanvas;
        public Canvas gameOverCanvas;

        public TextMeshProUGUI enemiesDefeatedText;
        public TextMeshProUGUI timeRemainingText;

        private CameraFollow cameraFollowScript;
        public int defeatedEnemies = 0;
        private float elapsedTime = 0f;

        void Start()
        {
            if (portal == null || bossArea == null || player == null || mainCamera == null || victoryCanvas == null || gameOverCanvas == null)
            {
                Debug.LogError("Algunas referencias no están asignadas en VictoryCondition. Revisa el inspector.");
                this.enabled = false;
                return;
            }

            victoryCanvas.gameObject.SetActive(false);
            gameOverCanvas.gameObject.SetActive(false);
            portal.gameObject.SetActive(false);
            boss.gameObject.SetActive(false);

            cameraFollowScript = mainCamera.GetComponent<CameraFollow>();
            UpdateUI();
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            float timeRemaining = Mathf.Max(0, timeLimit - elapsedTime);

            UpdateUI();

            if (timeRemaining <= 0)
            {
                Debug.Log("Tiempo agotado. ¡Perdiste!");
                if (gameOverCanvas != null)
                    gameOverCanvas.gameObject.SetActive(true);

                this.enabled = false;
                return;
            }

            if (defeatedEnemies >= requiredEnemies)
            {
                portal.gameObject.SetActive(true);
                StartCoroutine(MoveCameraToPortalAndSpawnBoss());
                this.enabled = false;
            }
        }

        public void OnEnemyDefeated()
        {
            defeatedEnemies++;
            Debug.Log($"Enemigo derrotado. Total: {defeatedEnemies}/{requiredEnemies}");
        }

        private void UpdateUI()
        {
            if (enemiesDefeatedText != null)
            {
                enemiesDefeatedText.text = $"Enemies Defeated: {defeatedEnemies} / {requiredEnemies}";
            }

            if (timeRemainingText != null)
            {
                float timeRemaining = Mathf.Max(0, timeLimit - elapsedTime);
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                timeRemainingText.text = $"Time Remaining: {minutes:D2}:{seconds:D2}";
            }
        }

        IEnumerator MoveCameraToPortalAndSpawnBoss()
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

            if (boss != null)
                boss.gameObject.SetActive(true);

            mainCamera.transform.position = originalPosition;
            mainCamera.transform.rotation = originalRotation;

            if (cameraFollowScript != null)
                cameraFollowScript.enabled = true;
        }

        public void OnBossDefeated()
        {
            Debug.Log("¡Jefe derrotado! Victoria.");
            if (victoryCanvas != null)
                victoryCanvas.gameObject.SetActive(true);

            this.enabled = false; // Detener lógica después de la victoria
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
