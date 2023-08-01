using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using CodesmithWorkshop.Useful;

public delegate void SetCrystalShardDelegate(CrystalShard crystal);
public delegate CrystalShard GetCrystalShardDelegate();

public class CrystalShardsManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private PoolingChannel m_poolingChannel = null;

    [SerializeField]
    private LevelChannel m_levelChannel = null;

    [SerializeField]
    private PoolConfig m_crystalPoolConfig = null;

    [SerializeField]
    private CrystalShardsSettings m_settings = null;

    private List<CrystalShard> m_crystals = new List<CrystalShard>();
    public List<CrystalShard> crystals
    {
        get { return m_crystals; }
    }

    [SerializeField]
    private Transform m_container;

    private List<CrystalShard> m_unavailableCrystals = new List<CrystalShard>();

    public List<CrystalShard> unavailableCrystals
    {
        get { return m_unavailableCrystals; }
    }

	#endregion

	#region Methods

    private IEnumerator Start()
    {
        m_levelChannel.crystalManager = this;
        ClearCrystalShards();

        yield return new WaitForEndOfFrame();
        m_poolingChannel.onCreatePool.Invoke(m_crystalPoolConfig);
    }

    private void OnDestroy() { }

    public void SpawnCrystalShards(SpawnCrystalShardsConfig config)
    {
        if (!Application.isPlaying)
            return;

        List<Vector3> randomPositions = LevelManager.RandomPositions(config.count, config.radius);

        for (int i = 0; i < config.count; i++)
        {
            int energy = Random.Range(config.minEnergy, config.maxEnergy + 1);
            SpawnCrystalShard(randomPositions[i], energy);
        }
    }

    private void SpawnCrystalShard(Vector3 position, int energy)
    {
        // Get From Pool
        CrystalShard crystal = m_poolingChannel.onGetFromPool
            .Invoke(m_crystalPoolConfig.prefab)
            .GetComponent<CrystalShard>();

        // Set Random Position
        crystal.transform.position = position;

        // Set Random Rotation
        Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
        crystal.transform.rotation = rotation;

        crystal.transform.localScale = Vector3.one * energy * m_settings.quantityToScaleRatio;

        crystal.Initialize(this, energy);
        m_crystals.Add(crystal);
    }

    /// <summary>
    /// Might be useful !
    /// </summary>
    private bool IsValidPosition(Vector3 position)
    {
        if (m_crystals.Count == 0)
            return true;

        float distance = 0;

        foreach (CrystalShard crystal in m_crystals)
        {
            distance = (position - crystal.transform.position).magnitude;

            if (distance < m_settings.minDistanceBetweenCrystals)
                return false;
        }

        return true;
    }

    public void AddUnavailableCrystal(CrystalShard crystal)
    {
        if (!m_unavailableCrystals.Contains(crystal))
            m_unavailableCrystals.Add(crystal);
    }

    public void RemoveUnavailableCrystal(CrystalShard crystal)
    {
        if (m_unavailableCrystals.Contains(crystal))
            m_unavailableCrystals.Remove(crystal);
    }

    public void DestroyCrystal(CrystalShard crystal)
    {
        if (m_crystals.Contains(crystal))
            m_crystals.Remove(crystal);

        RemoveUnavailableCrystal(crystal);

        m_poolingChannel.onReleaseFromPool(crystal.gameObject);
    }

    public void ClearCrystalShards()
    {
        UtilsClass.EmptyTransform(m_container, true);
        m_crystals.Clear();
    }

    #region Debug


    [SerializeField]
    private SpawnCrystalShardsConfig d_config;

    [ContextMenu("DebugSpawn")]
    public void DebugSpawn()
    {
        SpawnCrystalShards(d_config);
    }

    #endregion

	#endregion
}
