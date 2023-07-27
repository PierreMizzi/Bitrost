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
        SpawnFill();
    }

    private void OnDestroy() { }

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
        int quantity = UnityEngine.Random.Range(m_settings.minQuantity, m_settings.maxQuantity);

        crystal.transform.localScale = Vector3.one * quantity * m_settings.quantityToScaleRatio;

        crystal.Initialize(this, quantity);
        m_crystals.Add(crystal);
    }

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

    public CrystalShard GetRandomCrystal(List<CrystalShard> crystals)
    {
        int index = Random.Range(0, crystals.Count - 1);
        return crystals[index];
    }

    public void ClearCrystalShards()
    {
        UtilsClass.EmptyTransform(m_container, true);
        m_crystals.Clear();
    }

    [ContextMenu("Debug Fill Spawn")]
    public void DebugFillSpawn()
    {
        ClearCrystalShards();
        SpawnFill();
    }

	#endregion
}
