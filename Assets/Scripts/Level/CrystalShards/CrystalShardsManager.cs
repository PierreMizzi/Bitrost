using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PierreMizzi.Useful;
using PierreMizzi.Useful.PoolingObjects;
using Bitfrost.Gameplay.ScreenEdgeInfo;

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

        private List<CrystalShard> m_occupiedCrystals = new List<CrystalShard>();

        public List<CrystalShard> unavailableCrystals
        {
            get { return m_occupiedCrystals; }
        }

        [Header("Screen Edge Info")]
        [SerializeField]
        private ScreenEdgeSubject m_crystalsSubjectPrefab = null;

        #endregion

        #region Methods

        #region Restart

        private IEnumerator Start()
        {
            m_levelChannel.crystalManager = this;

            yield return new WaitForEndOfFrame();
            m_crystalPoolConfig.onGetFromPool = CrystalGetFromPool;
            m_poolingChannel.onCreatePool.Invoke(m_crystalPoolConfig);

            if (m_levelChannel != null)
                m_levelChannel.onReset += CallbackReset;
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
                m_levelChannel.onReset -= CallbackReset;
        }

        #endregion

        #region Spawning

        public void CrystalGetFromPool(GameObject gameObject)
        {
            CrystalShard crystal = gameObject.GetComponent<CrystalShard>();
            crystal.Hide();

            crystal.gameObject.SetActive(true);
        }

        public void SpawnCrystalShards(SpawnCrystalShardsConfig config)
        {
            if (!UnityEngine.Application.isPlaying)
                return;

            Vector3 origin = LevelManager.RandomPositionInArena(m_settings.randomPositionExtents);

            Vector3 randomPosition;
            Quaternion randomRotation = UtilsClass.RandomRotation2D();

            for (int i = 0; i < config.count; i++)
            {
                // Transforms
                randomPosition = GetSafePosition(origin);

                int energy = Random.Range(config.minEnergy, config.maxEnergy + 1);
                SpawnCrystalShard(randomPosition, randomRotation, energy);
            }

            // Spawn Crystal Edge
            Instantiate(m_crystalsSubjectPrefab, origin, Quaternion.identity, transform);
        }



        private void SpawnCrystalShard(Vector3 randomPosition, Quaternion randomRotation, int energyCount)
        {
            // Get From Pool
            CrystalShard crystal = m_poolingChannel.onGetFromPool
                .Invoke(m_crystalPoolConfig.prefab)
                .GetComponent<CrystalShard>();

            crystal.transform.SetPositionAndRotation(randomPosition, randomRotation);
            crystal.transform.localScale = Vector3.one * m_settings.GetRandomScale();

            crystal.OutOfPool(this, energyCount);
            m_activeCrystals.Add(crystal);
        }

        private Vector3 GetSafePosition(Vector3 origin)
        {
            Vector3 position = Vector3.one;
            Vector3 randomPosition;

            int generationCount = 0;
        GenerateRandom:
            {
                randomPosition = m_settings.GetRandomPosition();
                position = origin + randomPosition;
            }

            if (!IsValidPosition(position))
            {
                // Securit to prevent stalling the game
                generationCount++;
                if (generationCount < 20)
                    goto GenerateRandom;
                else
                    return position;
            }

            return position;
        }

        /// <summary>
        /// Might be useful !
        /// </summary>
        private bool IsValidPosition(Vector3 position)
        {
            if (!LevelManager.IsInsideArena(position))
                return false;

            if (m_activeCrystals.Count == 0)
                return true;

            float distance;
            float safeDistance;

            foreach (CrystalShard crystal in m_activeCrystals)
            {
                distance = (position - crystal.transform.position).sqrMagnitude;
                safeDistance = m_settings.SafeDistanceFromScale(crystal.transform.localScale.x);

                if (distance < safeDistance)
                    return false;
            }

            return true;
        }

        #endregion



        public void AddOccupiedCrystal(CrystalShard crystal)
        {
            if (!m_occupiedCrystals.Contains(crystal))
                m_occupiedCrystals.Add(crystal);
        }

        public void RemoveOccupiedCrystal(CrystalShard crystal)
        {
            if (m_occupiedCrystals.Contains(crystal))
                m_occupiedCrystals.Remove(crystal);
        }

        public void ReleaseCrystal(CrystalShard crystal)
        {
            if (m_activeCrystals.Contains(crystal))
                m_activeCrystals.Remove(crystal);

            RemoveOccupiedCrystal(crystal);

            m_poolingChannel.onReleaseToPool(crystal.gameObject);
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
                m_poolingChannel.onReleaseToPool(crystal.gameObject);
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
