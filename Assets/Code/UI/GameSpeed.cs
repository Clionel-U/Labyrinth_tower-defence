using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour
{
    public TMP_Text speedText;
    public Image pauseButton;
    public Sprite pause;
    public Sprite play;
    float speedOfTime = 1;
    public TMP_Text testText;
    public GameObject pauseOverlay;

    private void Awake()
    {
        Time.timeScale = 1;
        speedText.text = "X1";
    }
    public void ChangeSpeed()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
            speedText.text = "X2";
        }
        else if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
            speedText.text = "X1";
        }
        else return;
    }
    public void Pause()
    {
        if (Time.timeScale != 0)
        {
            speedOfTime = Time.timeScale;
            Time.timeScale = 0;
            pauseButton.sprite = play;
            pauseOverlay.SetActive(true);
        }
        else
        {
            Time.timeScale = speedOfTime;
            pauseButton.sprite = pause;
            pauseOverlay.SetActive(false);
        }
    }

    private void Update()
    {
        testText.text = $"Time speed  {Time.timeScale}";
    }
}