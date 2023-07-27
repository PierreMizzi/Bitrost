using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;

public delegate void InstantiateBulletDelegate(
    IBulletLauncher launcher,
    Bullet prefab,
    Vector3 position,
    Vector3 orientation
);

public delegate void ReleaseBulletDelegate(Bullet bullet);

public class BulletManager : MonoBehaviour
{

    [SerializeField]
    private BulletChannel m_bulletChannel = null;

    [SerializeField]
    private PoolingChannel m_poolingChannel = null;

    private IEnumerator Start()
    {
        if (m_bulletChannel != null)
        {
            m_bulletChannel.onInstantiateBullet += CallbackInstantiateBullet;
            m_bulletChannel.onReleaseBullet += CallbackReleaseBullet;
        }

        yield return new WaitForEndOfFrame();

        InitializeBulletPools();
    }

    private void OnDestroy()
    {
        if (m_bulletChannel != null)
        {
            m_bulletChannel.onInstantiateBullet -= CallbackInstantiateBullet;
            m_bulletChannel.onReleaseBullet -= CallbackReleaseBullet;
        }
    }

    private void InitializeBulletPools()
    {
        foreach (BulletPoolConfig config in m_bulletChannel.bulletPoolConfigs)
            m_poolingChannel.onCreatePool.Invoke(config);
    }

    public void CallbackInstantiateBullet(
        IBulletLauncher launcher,
        Bullet prefab,
        Vector3 position,
        Vector3 orientation
    )
    {
        Bullet bullet = m_poolingChannel.onGetFromPool
            .Invoke(prefab.gameObject)
            .GetComponent<Bullet>();

        bullet.AssignLauncher(launcher);
        bullet.transform.position = position;
        bullet.transform.up = orientation;
    }

    public void CallbackReleaseBullet(Bullet bullet)
    {
        m_poolingChannel.onReleaseFromPool.Invoke(bullet.gameObject);
    }
}
