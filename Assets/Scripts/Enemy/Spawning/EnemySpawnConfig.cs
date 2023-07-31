using System;

[Serializable]
public struct EnemySpawnConfig
{

	public Enemy prefab;
    public int minDelaySpawn;
    public int maxDelaySpawn;

    public int maxCount;

	public bool autoReplace;
}
