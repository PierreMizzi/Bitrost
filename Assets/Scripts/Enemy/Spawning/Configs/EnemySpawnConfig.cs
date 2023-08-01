using System;
using UnityEngine;

[Serializable]
public struct EnemySpawnConfig
{
    public Enemy prefab;

    public int count;

    [Range(1, 10)]
    public int batchCount;

    [HideInInspector]
    public float duration;

    public float spawnFrequency
    {
        get { return (duration / count) * batchCount; }
    }

}
