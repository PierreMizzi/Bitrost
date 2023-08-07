using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PierreMizzi.Useful;
using PierreMizzi.Useful.PoolingObjects;



namespace Bitfrost.Gameplay.Energy
{
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

        private List<CrystalShard> m_activeCrystals = new List<CrystalShard>();
        public List<CrystalShard> activeCrystals
        {
            get { return m_activeCrystals; }
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

        #region Restart

        private IEnumerator Start()
        {
            m_levelChannel.crystalManager = this;

            yield return new WaitForEndOfFrame();
            m_poolingChannel.onCreatePool.Invoke(m_crystalPoolConfig);

            if (m_levelChannel != null)
                m_levelChannel.onReset += CallbackReset;


            DebugSpawn();
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
                m_levelChannel.onReset -= CallbackReset;
        }

        #endregion

        #region Spawning

        public void SpawnCrystalShards(SpawnCrystalShardsConfig config)
        {
            if (!UnityEngine.Application.isPlaying)
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
            m_activeCrystals.Add(crystal);
        }


        #endregion


        /// <summary>
        /// Might be useful !
        /// </summary>
        private bool IsValidPosition(Vector3 position)
        {
            if (m_activeCrystals.Count == 0)
                return true;

            float distance = 0;

            foreach (CrystalShard crystal in m_activeCrystals)
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
            if (m_activeCrystals.Contains(crystal))
                m_activeCrystals.Remove(crystal);

            RemoveUnavailableCrystal(crystal);

            m_poolingChannel.onReleaseFromPool(crystal.gameObject);
        }

        public void ClearCrystalShards()
        {
            UtilsClass.EmptyTransform(m_container, true);
            m_activeCrystals.Clear();
        }

        #region Reset

        public void CallbackReset()
        {
            foreach (CrystalShard crystal in m_activeCrystals)
            {
                crystal.Reset();
                m_poolingChannel.onReleaseFromPool(crystal.gameObject);
            }

            m_activeCrystals.Clear();
        }

        #endregion

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
}
