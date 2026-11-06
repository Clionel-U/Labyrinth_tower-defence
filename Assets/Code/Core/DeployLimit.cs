using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeployLimit : MonoBehaviour
{
    public static DeployLimit Instance;

    public TMP_Text text;
    public int deploymentLimit;
    public int currentLimit;

    void Awake()
    {
        Instance = this;
    
        currentLimit = deploymentLimit;
        UpdateText();
    }

    public void PlusLimit()
    {
        currentLimit++;
        if (currentLimit > deploymentLimit) currentLimit = deploymentLimit;
        UpdateText();
    }

    public void MinusLimit()
    {
        currentLimit--;
        if (currentLimit < 0) currentLimit = 0;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = $"Лимит юнитов: {currentLimit}";
    }
}
