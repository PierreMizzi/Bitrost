using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PierreMizzi.Useful.PoolingObjects;

namespace Bitfrost.Gameplay.Bullets
{

    public delegate void InstantiateBulletDelegate(
    IBulletLauncher launcher,
    Bullet prefab,
    Vector3 position,
    Vector3 orientation
);

    public delegate void ReleaseBulletDelegate(Bullet bullet);

    public class BulletManager : MonoBehaviour, IPausable
    {
        [Header("Channels")]
        [SerializeField]
        private BulletChannel m_bulletChannel = null;

        [SerializeField]
        private PoolingChannel m_poolingChannel = null;

        [SerializeField]
        private LevelChannel m_levelChannel = null;

        private List<Bullet> m_activeBullets = new List<Bullet>();

        public bool isPaused { get; set; }


        #region MonoBehaviour

        private IEnumerator Start()
        {
            if (m_bulletChannel != null)
            {
                m_bulletChannel.onInstantiateBullet += CallbackInstantiateBullet;
                m_bulletChannel.onReleaseBullet += CallbackReleaseBullet;
            }

            if (m_levelChannel != null)
            {
                m_levelChannel.onPauseGame += Pause;
                m_levelChannel.onResumeGame += Resume;
                m_levelChannel.onReset += CallbackReset;
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

            if (m_levelChannel != null)
            {
                m_levelChannel.onPauseGame -= Pause;
                m_levelChannel.onResumeGame -= Resume;
                m_levelChannel.onReset -= CallbackReset;
            }
        }

        #endregion



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

            if (!m_activeBullets.Contains(bullet))
                m_activeBullets.Add(bullet);
        }

        public void CallbackReleaseBullet(Bullet bullet)
        {
            if (m_activeBullets.Contains(bullet))
                m_activeBullets.Remove(bullet);
            m_poolingChannel.onReleaseToPool.Invoke(bullet.gameObject);
        }

        #region Reset

        public void CallbackReset()
        {
            foreach (Bullet bullet in m_activeBullets)
                m_poolingChannel.onReleaseToPool.Invoke(bullet.gameObject);

            m_activeBullets.Clear();
        }

        #endregion

        #region Pause

        public void Pause()
        {
            isPaused = true;
            foreach (Bullet bullet in m_activeBullets)
                bullet.Pause();
        }

        public void Resume()
        {
            isPaused = false;
            foreach (Bullet bullet in m_activeBullets)
                bullet.Resume();
        }

        #endregion
    }
}
