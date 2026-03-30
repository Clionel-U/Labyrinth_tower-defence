using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int SP;
    public void Activate()
    {
        Debug.Log("Skill activated");

        // пример
        SP = 0;

        // тут логика скилла
    }
    //public int SP;
    //public int maxSP = 10;

    //public EntityData unit;

    //private Coroutine SPgainCoroutine;
    //private bool skillReady = false;
    //private bool skillActive = false;

    //private void Start()
    //{
    //    unit = GetComponent<EntityData>();
    //    SPgainCoroutine = StartCoroutine(SPgain());
    //}

    //IEnumerator SPgain()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1);

    //        SP++;

    //        if (SP >= maxSP)
    //        {
    //            SP = maxSP;
    //            skillReady = true;

    //            StopCoroutine(SPgainCoroutine);
    //            SPgainCoroutine = null;

    //            yield break; // ВАЖНО!
    //        }
    //    }
    //}

    //public void ActivateSkill()
    //{
    //    if (!skillReady || skillActive) return;

    //    StartCoroutine(SkillActive());
    //}

    //IEnumerator SkillActive()
    //{
    //    skillActive = true;
    //    skillReady = false;

    //    int buff = unit.baseATK;
    //    unit.ATK += buff;

    //    yield return new WaitForSeconds(5);

    //    unit.ATK -= buff;

    //    SP = 0;
    //    skillActive = false;

    //    SPgainCoroutine = StartCoroutine(SPgain());
    //}


    //public int SP;
    //public EntityData unit;
    //private Coroutine SPgainCoroutine;

    //private void Start()
    //{
    //    unit = GetComponent<EntityData>();
    //    SPgainCoroutine = StartCoroutine(SPgain());
    //}

    //void Update()
    //{
    //    if (SP >= 10)
    //    {
    //        StopCoroutine(SPgainCoroutine);
    //        SP = 0;
    //        StartCoroutine(SkillActive(unit));

    //    }
    //}

    //IEnumerator SPgain()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1);
    //        SP += 1;
    //    }
    //}

    //IEnumerator SkillActive(EntityData unit)
    //{
    //    int buff = unit.ATK;
    //    unit.ATK += buff;
    //    yield return new WaitForSeconds(5);
    //    unit.ATK -= buff;
    //    SPgainCoroutine = StartCoroutine(SPgain());
    //}
}
