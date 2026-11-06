using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [Header("��������� �������")]
    public float fadeDuration = 0.5f; // ������������ ��������� � ��������
    public Color spawnColor = Color.black; // ������ � ������� (����� ������� ����������)

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isSpawned = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // ���������� "������" ���� �� ����������
            spriteRenderer.color = spawnColor; // ��������� ������ ������
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

        // ������ ������ ���� �� spawnColor (�������) � originalColor (�������)
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            // ����������� ������������� ���� � ������� Lerp
            Color current = Color.Lerp(spawnColor, originalColor, elapsed / fadeDuration);
            spriteRenderer.color = current;

            yield return null; // ���� ��������� ����
        }

        // �����������, ��� � ����� ����������� ������ ������������ ����
        spriteRenderer.color = originalColor;
    }
}