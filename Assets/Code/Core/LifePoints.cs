using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePoints : MonoBehaviour
{
    [Header("���������")]
    public int maxLives;
    public int currentLives;

    [Header("������")]
    public WaveManager enemyManager; // ������ �� ������, ��� ��������� EnemyReachedGoal
    public TMPro.TMP_Text livesText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }

    private void OnEnable()
    {
        // ������������� �� �������
        if (enemyManager != null)
            enemyManager.OnReachedGoal += TakeDamage;
    }

    private void OnDisable()
    {
        // ������������ (������� ��� ��� �������������� ������ ������)
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
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("���� ��������!");
    }
}
