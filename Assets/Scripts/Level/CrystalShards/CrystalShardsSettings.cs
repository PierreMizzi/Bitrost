using UnityEngine;

[CreateAssetMenu(
    fileName = "CrystalShardsSettings",
    menuName = "Bitrost/CrystalShardsSettings",
    order = 0
)]
public class CrystalShardsSettings : ScriptableObject
{
	// [Header("Spawning Zone")]

	public float minDistanceBetweenCrystals;

    [Header("Fill spawning")]
    public int minFillSpawnCount = 20;
    public int maxFillSpawnCount = 40;

    [Header("Continuous Spawning")]
    public float minDelayContinuousSpawn = 10f;
    public float maxDelayContinuousSpawn = 30f;

	[Header("Individual crystal")]
    public int minQuantity = 25;
    public int maxQuantity = 100;

    public float quantityToScaleRatio = 0.033f;
}
