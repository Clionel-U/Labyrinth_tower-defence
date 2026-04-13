using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    public Image faderImage;
    public float fadeDuration;

    private void Start()
    {
        // При старте сцены плавно проявляем её из черного
        StartCoroutine(Fade(1, 0));
    }

    public void ToScene(int sceneIndex)
    {
        StartCoroutine(SwitchScene(sceneIndex));
    }

    private IEnumerator SwitchScene(int index)
    {
        // Плавно затемняем экран
        yield return StartCoroutine(Fade(0, 1));
        // Загружаем новую сцену
        SceneManager.LoadScene(index);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        faderImage.color = new Color(0, 0, 0, startAlpha);
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            faderImage.color = new Color(0, 0, 0, a);
            yield return null;
        }
        faderImage.color = new Color(0, 0, 0, endAlpha);
    }
}