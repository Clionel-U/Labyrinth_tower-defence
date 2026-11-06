using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [Header("Настройки эффекта")]
    public float fadeDuration = 0.5f; // Длительность появления в секундах
    public Color spawnColor = Color.black; // Начнем с черного (можно сделать прозрачным)

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isSpawned = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Запоминаем "родной" цвет из инспектора
            spriteRenderer.color = spawnColor; // Мгновенно делаем черным
        }
        else enabled = false;
    }

    private void Start()
    {
        if (!isSpawned)
        {
            StartCoroutine(SpawnFadeRoutine());
        }
    }

    private IEnumerator SpawnFadeRoutine()
    {
        isSpawned = true;
        float elapsed = 0f;

        // Плавно меняем цвет от spawnColor (черного) к originalColor (родному)
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            // Расчитываем промежуточный цвет с помощью Lerp
            Color current = Color.Lerp(spawnColor, originalColor, elapsed / fadeDuration);
            spriteRenderer.color = current;

            yield return null; // Ждем следующий кадр
        }

        // Гарантируем, что в конце установится точный оригинальный цвет
        spriteRenderer.color = originalColor;
    }
}