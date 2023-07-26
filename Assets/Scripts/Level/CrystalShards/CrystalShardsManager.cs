using UnityEngine;
using System.Collections.Generic;
using CodesmithWorkshop.Useful;

public class CrystalShardsManager : MonoBehaviour
{
	#region Fields


    [SerializeField]
    private PoolingChannel m_poolingChannel = null;

    [SerializeField]
    private PoolConfig m_crystalPoolConfig = null;

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

        m_poolingChannel.onCreatePool.Invoke(m_crystalPoolConfig);
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
        // Get From Pool
        CrystalShard crystal = m_poolingChannel.onGetFromPool
            .Invoke(m_crystalPoolConfig.prefab)
            .GetComponent<CrystalShard>();

        // Set Random Position
        if (position == Vector3.zero)
            position = LevelManager.RandomPosition();
        crystal.transform.position = position;

        // Set Random Rotation
        Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
        crystal.transform.rotation = rotation;

        // set random quantity
        int quantity = UnityEngine.Random.Range(
            m_settings.minQuantity,
            m_settings.maxQuantity
        );

        crystal.transform.localScale =
            Vector3.one * quantity * m_settings.quantityToScaleRatio;
            
        crystal.Initialize(this, quantity);
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
