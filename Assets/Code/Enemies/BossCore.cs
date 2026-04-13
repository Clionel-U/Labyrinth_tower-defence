using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : MonoBehaviour
{
    private EntityData self;
    public GameObject block;

    [Header("Состояние")]
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
        // 1. Делаем босса временно неуязвимым и неподвижным
        self.isInvulnerable = true;
        self.speed = 0;

        // 2. Визуальный эффект (например, босс стоит на коленях и восстанавливается)
        yield return new WaitForSeconds(3f);

        // 3. Обновляем статы
        self.HP = self.maxHP;
        self.isInvulnerable = false;
        block.SetActive(false); // Убираем блок
        self.speed = stage2speed; // Меняем скорость во второй стадии
        Stage2?.Invoke();

        Debug.Log("вторая стадия");
    }
}
