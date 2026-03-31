using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BitiumSystem : MonoBehaviour
{
    public static BitiumSystem Instance;
    void Awake() => Instance = this;

    public bool bitiumGain;
    public int bitium = 0;
    public TMP_Text bitiumCounter;
    public event System.Action OnBitiumChanged;

    void Start()
    {
        bitiumCounter.text = $"{bitium}";
        if (bitiumGain) 
            StartCoroutine(BitiumGain());
    }

    public void BitiumChange()
    {
        bitiumCounter.text = $"{bitium}";
        OnBitiumChanged?.Invoke();
    }

    IEnumerator BitiumGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            bitium += 1;
            BitiumChange();
        }
    }
}
