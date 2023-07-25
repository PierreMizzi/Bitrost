using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public delegate void InstantiateBulletDelegate(
    IBulletLauncher launcher,
    BulletType type,
    Vector3 position,
    Vector3 orientation
);

public delegate void ReleaseBulletDelegate(Bullet bullet);

public class BulletManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private BulletChannel m_bulletChannel = null;

    [SerializeField]
    private List<BulletPoolConfig> m_poolConfigs = new List<BulletPoolConfig>();

    [SerializeField]
    private Transform m_poolsContainer = null;

    private Dictionary<BulletType, ObjectPool<Bullet>> m_bulletPools =
        new Dictionary<BulletType, ObjectPool<Bullet>>();

	#endregion

	#region Methods

    public void Awake()
    {
        foreach (BulletPoolConfig config in m_poolConfigs)
            InitializePool(config);
    }

    private void Start()
    {
        if (m_bulletChannel != null)
        {
            m_bulletChannel.onInstantiateBullet += CallbackInstantiateBullet;
            m_bulletChannel.onReleaseBullet += CallbackReleaseBullet;
        }
    }

    private void OnDestroy()
    {
        if (m_bulletChannel != null)
        {
            m_bulletChannel.onInstantiateBullet -= CallbackInstantiateBullet;
            m_bulletChannel.onReleaseBullet -= CallbackReleaseBullet;
        }
    }

    private void InitializePool(BulletPoolConfig config)
    {
        if (!m_bulletPools.ContainsKey(config.prefab.type))
        {
            // Creates a container
            GameObject container = new GameObject();
            container.name = $"PoolContainer_{config.prefab.type}";
            container.transform.parent = m_poolsContainer;

            // Creates a special method for creating pool instances
            System.Func<Bullet> create = () =>
            {
                return Instantiate(
                    config.prefab,
                    Vector3.zero,
                    Quaternion.identity,
                    container.transform
                );
            };

            // Create a new pool
            ObjectPool<Bullet> pool = new ObjectPool<Bullet>(
                create,
                GetFromPool,
                ReleaseToPool,
                DestroyFromPool,
                true,
                config.defaultSize,
                config.maxSize
            );

            // Add the pool in the dictionnary
            m_bulletPools.Add(config.prefab.type, pool);
        }
    }

    private void ReleaseToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void GetFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void DestroyFromPool(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void CallbackInstantiateBullet(IBulletLauncher launcher, BulletType type, Vector3 position, Vector3 orientation)
    {
        if (m_bulletPools.ContainsKey(type))
        {
            Bullet bullet = m_bulletPools[type].Get();
            bullet.launcher = launcher;
            bullet.transform.position = position;
            bullet.transform.up = orientation;
        }
    }

    public void CallbackReleaseBullet(Bullet bullet)
    {
        if (m_bulletPools.ContainsKey(bullet.type))
            m_bulletPools[bullet.type].Release(bullet);
    }

	#endregion
}
