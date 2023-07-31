using System;

[Serializable]
public struct EnemySpawnConfig
{
    public Enemy prefab;

    public int maxCount;
    public int batchCount;
    public float duration;

    public float spawnFrequency
    {
        get { return (duration / maxCount) * batchCount; }
    }

}
