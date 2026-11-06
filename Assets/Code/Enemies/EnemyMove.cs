using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private EnemyPath path;
    private int currentIndex = 1; // сразу к следующей точке
    private EnemyBlock block;
    private EntityData data;
    private SpriteRenderer sr;

    public bool stopped = false;
    public float stopDuration = 0f;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    public void Init(EnemyPath _path)
    {
        path = _path;
        data = GetComponent<EntityData>();
        block = GetComponentInChildren<EnemyBlock>();
        currentIndex = 1;
    }

    void Update()
    {
        if (path == null || data == null) return;
        if (block != null && block.IsBlocked()) return;
        if (stopped)
        {
            if (stopDuration > 0)
            {
                stopDuration -= Time.deltaTime;
                return;
            }
            else stopped = false;
        }
        Move();
    }

    void Move()
    {
        if (currentIndex >= path.Length) return;

        Transform target = path.GetPoint(currentIndex);
        if (target == null) return;

        float speed = data.speed;
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // ѕоворот по направлению
        Vector3 dir = (target.position - transform.position).normalized;
        if (dir.x != 0)
            sr.flipX = dir.x < 0;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= path.Length)
                ReachGoal();
        }
    }

    void ReachGoal()
    {
        WaveManager.Instance.EnemyReachedGoal(gameObject);
        Destroy(gameObject);
    }

    public void StopEnemy(float duration)
    {
        stopped = true;
        stopDuration = duration;
    }
}