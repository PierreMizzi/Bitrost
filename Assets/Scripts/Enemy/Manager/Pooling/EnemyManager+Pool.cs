using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public partial class EnemyManager
{
	#region Fields

    [SerializeField]
    private List<EnemyPoolConfig> m_poolConfigs = new List<EnemyPoolConfig>();

    [SerializeField]
    private Transform m_poolsContainer = null;

    private Dictionary<EnemyType, ObjectPool<Enemy>> m_enemyPools =
        new Dictionary<EnemyType, ObjectPool<Enemy>>();

	#endregion

	#region Methods

    public void Awake()
    {
        foreach (EnemyPoolConfig config in m_poolConfigs)
            InitializePool(config);
    }

    private void InitializePool(EnemyPoolConfig config)
    {
        if (!m_enemyPools.ContainsKey(config.prefab.type))
        {
            // Creates a container
            GameObject container = new GameObject();
            container.name = $"PoolContainer_{config.prefab.type}";
            container.transform.parent = m_poolsContainer;

            // Creates a special method for creating pool instances
            System.Func<Enemy> create = () =>
            {
                return Instantiate(
                    config.prefab,
                    Vector3.zero,
                    Quaternion.identity,
                    container.transform
                );
            };

            // Create a new pool
            ObjectPool<Enemy> pool = new ObjectPool<Enemy>(
                create,
                GetFromPool,
                ReleaseToPool,
                DestroyFromPool,
                true,
                config.defaultSize,
                config.maxSize
            );

            // Add the pool in the dictionnary
            m_enemyPools.Add(config.prefab.type, pool);
        }
    }

    private void ReleaseToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void GetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void DestroyFromPool(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    public Enemy GetEnemyFromType(EnemyType type)
    {
        if (m_enemyPools.ContainsKey(type))
            return m_enemyPools[type].Get();
        else
            return null;
    }

	#endregion
}
