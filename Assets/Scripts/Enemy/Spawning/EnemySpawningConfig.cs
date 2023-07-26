using System;

[Serializable]
public struct EnemySpawningConfig
{
    public Enemy prefab;

    public int minSpawnDelay;
    public int maxSpawnDelay;
}
