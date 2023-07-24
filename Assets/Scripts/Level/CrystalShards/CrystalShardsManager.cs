using UnityEngine;
using System.Collections.Generic;
using CodesmithWorkshop.Useful;

public class CrystalShardsManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private CrystalShard m_crystalPrefab = null;

    [SerializeField]
    private CrystalShardsSettings m_settings = null;

    private List<CrystalShard> m_crystalShards = new List<CrystalShard>();

    [SerializeField]
    private Transform m_container;

	#endregion

	#region Methods

    private void Start()
    {
		ClearCrystalShards();
        SpawnFill();
    }

    private void SpawnFill()
    {
        int count = UnityEngine.Random.Range(
            m_settings.minFillSpawnCount,
            m_settings.maxFillSpawnCount
        );

        for (int i = 0; i < count; i++)
            SpawnCrystalShard();
    }

    private void SpawnContinuous() { }

    private void SpawnCrystalShard(Vector3 position = new Vector3())
    {
        if (position == Vector3.zero)
            position = LevelManager.RandomPosition();

        Quaternion randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));

        CrystalShard crystal = Instantiate(m_crystalPrefab, position, randomRotation, m_container);

        int randomQuantity = UnityEngine.Random.Range(
            m_settings.minQuantity,
            m_settings.maxQuantity
        );

        crystal.transform.localScale =
            Vector3.one * randomQuantity * m_settings.quantityToScaleRatio;
        crystal.Initialize(this, randomQuantity);
        m_crystalShards.Add(crystal);
    }

    private bool IsValidPosition(Vector3 position)
    {
        if (m_crystalShards.Count == 0)
            return true;

        float distance = 0;

        foreach (CrystalShard crystal in m_crystalShards)
        {
            distance = (position - crystal.transform.position).magnitude;

            if (distance < m_settings.minDistanceBetweenCrystals)
                return false;
        }

        return true;
    }

    public void ClearCrystalShards()
    {
        UtilsClass.EmptyTransform(m_container, true);
        m_crystalShards.Clear();
    }

    [ContextMenu("Debug Fill Spawn")]
    public void DebugFillSpawn()
    {
		ClearCrystalShards();
        SpawnFill();
    }

	#endregion
}
