using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Move : MonoBehaviour
{
    public EntityData self;
    public EnemyBlock block;

    private void OnEnable()
    {
        self = GetComponent<EntityData>();
        block = GetComponent<EnemyBlock>();

        
    }
    void Update()
    {
        if (block != null && !block.IsBlocked())
        {
            transform.Translate(Vector2.down * self.speed * Time.deltaTime);
        }
    }
}
