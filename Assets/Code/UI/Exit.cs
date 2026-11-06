using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public SceneFade sf;
    public void Restart()
    {
        // ��������� ����� � �����
        sf.ToScene(1);
    }

    public void ExitLevel()
    {
        sf.ToScene(0);
    }
}
