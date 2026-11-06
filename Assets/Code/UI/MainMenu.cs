using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneFade sf;
    public void Play()
    {
        // ��������� ����� � �����
        sf.ToScene(1);
    }

    public void QuitGame()
    {
        Application.Quit(); // ��������� ���������� (�������� � �����)
    }
}