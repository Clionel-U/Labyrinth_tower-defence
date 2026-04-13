using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;
    public int pathID;
    public float delay;
}

[System.Serializable]
public class WaveData
{
    public string waveName;
    public List<EnemySpawnEntry> enemies = new List<EnemySpawnEntry>();
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    void Awake() => Instance = this;

    public System.Action OnAllWavesCompleted;
    public System.Action<int> OnReachedGoal;

    [Header("Волны")]
    public List<WaveData> waves = new List<WaveData>();

    [Header("Пути")]
    public List<EnemyPath> paths = new List<EnemyPath>();

    [Header("UI")]
    public TMP_Text killCounterLabel; // "34 / 57"

    [Header("Настройки")]
    public float timeBeforeFirstWave = 5f;
    public float timeBetweenWaves = 10f;

    private int totalEnemies = 0;  // всего врагов за уровень
    private int killedEnemies = 0; // сколько убито
    private int activeEnemies = 0; // живые на сцене прямо сейчас

    void Start()
    {
        // Считаем всех врагов за уровень заранее
        foreach (var wave in waves)
            totalEnemies += wave.enemies.Count;

        UpdateKillCounter();
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(timeBeforeFirstWave);

        for (int i = 0; i < waves.Count; i++)
        {
            yield return StartCoroutine(SpawnWave(waves[i]));
            yield return new WaitUntil(() => activeEnemies <= 0);

            if (i < waves.Count - 1)
                yield return new WaitForSeconds(timeBetweenWaves);
        }

        OnAllWavesComplete();
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        var sorted = new List<EnemySpawnEntry>(wave.enemies);
        sorted.Sort((a, b) => a.delay.CompareTo(b.delay));

        float elapsed = 0f;
        int i = 0;

        while (i < sorted.Count)
        {
            if (elapsed >= sorted[i].delay)
            {
                SpawnEnemy(sorted[i]);
                i++;
            }
            else
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    void SpawnEnemy(EnemySpawnEntry entry)
    {
        EnemyPath path = paths.Find(p => p.pathID == entry.pathID);
        if (path == null || path.Length == 0)
        {
            Debug.LogWarning($"Путь с ID {entry.pathID} не найден!");
            return;
        }

        GameObject enemy = Instantiate(
            entry.enemyPrefab,
            path.points[0].position,
            Quaternion.identity
        );

        enemy.GetComponent<EnemyMove>()?.Init(path);

        activeEnemies++;
        EntityData data = enemy.GetComponent<EntityData>();
        if (data != null)
        {
            data.OnDeath += () =>
            {
                activeEnemies--;
                killedEnemies++;
                UpdateKillCounter();
            };
        }
    }

    public void EnemyReachedGoal(GameObject enemy)
    {   
        activeEnemies--;
        killedEnemies++;
        if (enemy.GetComponent<EntityData>().isBoss)
        {
            OnReachedGoal?.Invoke(10);
        }
        else OnReachedGoal?.Invoke(1);
        UpdateKillCounter();
    }

    void UpdateKillCounter()
    {
        if (killCounterLabel != null)
            killCounterLabel.text = $"{killedEnemies} / {totalEnemies}";
    }

    void OnAllWavesComplete()
    {
        OnAllWavesCompleted?.Invoke();
        Debug.Log("Все волны завершены!");
    }
}