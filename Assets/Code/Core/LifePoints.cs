using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePoints : MonoBehaviour
{
    [Header("Настройки")]
    public int maxLives;
    public int currentLives;

    [Header("Ссылки")]
    public WaveManager enemyManager; // Ссылка на скрипт, где находится EnemyReachedGoal
    public TMPro.TMP_Text livesText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }

    private void OnEnable()
    {
        // Подписываемся на событие
        if (enemyManager != null)
            enemyManager.OnReachedGoal += TakeDamage;
    }

    private void OnDisable()
    {
        // Отписываемся (хороший тон для предотвращения утечек памяти)
        if (enemyManager != null)
            enemyManager.OnReachedGoal -= TakeDamage;
    }

    private void TakeDamage(int damage)
    {
        if (currentLives <= 0) return;

        currentLives -= damage;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"{currentLives}";
    }

    private void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("Игра окончена!");
    }
}
