using System;

[Serializable]
public struct EnemySpawningConfig
{
    public EnemyType type;

    public int minSpawnDelay;
    public int maxSpawnDelay;
}
