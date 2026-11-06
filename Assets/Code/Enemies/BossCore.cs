using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : MonoBehaviour
{
    private EntityData self;
    public GameObject block;
    public SpriteRenderer range;

    [Header("���������")]
    public int currentStage = 1;
    public float stage2speed = 0.3f;
    //public bool isDead = false;

    public System.Action Stage2;

    void Awake()
    {
        self = GetComponent<EntityData>();
        self.isBoss = true;
    }

    public void HandleDeath()
    {
        if (currentStage == 1)
        {
            StartCoroutine(TransitionToStageTwo());
        }
        else
        {
            self.Die();
        }
    }

    private IEnumerator TransitionToStageTwo()
    {
        currentStage = 2;
        // 1. ������ ����� �������� ���������� � �����������
        self.isInvulnerable = true;
        self.speed = 0;

        // 2. ���������� ������ (��������, ���� ����� �� ������� � �����������������)
        yield return new WaitForSeconds(3f);

        // 3. ��������� �����
        self.HP = self.maxHP;
        self.isInvulnerable = false;
        block.SetActive(false); // ������� ����
        self.speed = stage2speed; // ������ �������� �� ������ ������
        Stage2?.Invoke();
        range.enabled = true;

        Debug.Log("������ ������");
    }
}
