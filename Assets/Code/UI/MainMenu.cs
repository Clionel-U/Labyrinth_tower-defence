using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneFade sf;
    public void Play()
    {
        // Загружаем сцену с игрой
        sf.ToScene(1);
    }

    public void QuitGame()
    {
        Application.Quit(); // Закрывает приложение (работает в билде)
    }
}