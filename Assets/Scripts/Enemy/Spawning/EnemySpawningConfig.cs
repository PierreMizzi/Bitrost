using System;
using UnityEngine;

[Serializable]
public struct EnemySpawningConfig
{
    public Enemy prefab;

    public int minSpawnDelay;
    public int maxSpawnDelay;

    public KeyCode quickSpawnKey;
}
