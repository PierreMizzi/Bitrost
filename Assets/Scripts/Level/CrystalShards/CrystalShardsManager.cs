using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PierreMizzi.Useful;
using PierreMizzi.Useful.PoolingObjects;
using Bitfrost.Gameplay.ScreenEdgeInfo;
using System;
using PierreMizzi.Pause;

namespace Bitfrost.Gameplay.Energy
{
    public delegate void SetCrystalShardDelegate(CrystalShard crystal);

    public delegate CrystalShard GetCrystalShardDelegate();

    /// <summary>
    /// Manages crystal shards : spawning according to timeline stage, pooling, releasing, pausing etc
    /// </summary>
    public class CrystalShardsManager : MonoBehaviour, IPausable
    {
        #region Fields

        [Header("Channels")]
        [SerializeField]
        private PoolingChannel m_poolingChannel = null;

        [SerializeField]
        private LevelChannel m_levelChannel = null;

        [SerializeField]
        private CrystalShardsChannel m_crystalChannel = null;

        [SerializeField]
        private List<PoolConfig> m_crystalPoolConfigs = new List<PoolConfig>();

        [SerializeField]
        private CrystalShardsSettings m_settings = null;

        /// <summary>
        /// Crystal Shards currently out of their pool and active in-game
        /// </summary>
        private List<CrystalShard> m_activeCrystals = new List<CrystalShard>();

        /// <summary>
        /// Crystal Shards currently occupied by a turret
        /// </summary>
        private List<CrystalShard> m_occupiedCrystals = new List<CrystalShard>();

        [Header("Screen Edge Info")]
        [SerializeField]
        private ScreenEdgeSubject m_crystalsSubjectPrefab = null;

        [Header("Debug")]
        [SerializeField]
        private SpawnCrystalShardsConfig d_config;

        public bool isPaused { get; set; }

        #endregion

        #region Methods

        #region MonoBehaviour

        private IEnumerator Start()
        {
            m_levelChannel.crystalManager = this;

            yield return new WaitForEndOfFrame();

            // New
            foreach (PoolConfig poolConfig in m_crystalPoolConfigs)
            {
                poolConfig.onGetFromPool = GetCrystalFromPool;
                m_poolingChannel.onCreatePool.Invoke(poolConfig);
            }

            if (m_levelChannel != null)
            {
                m_levelChannel.onReset += CallbackReset;
                m_levelChannel.onPauseGame += Pause;
                m_levelChannel.onResumeGame += Resume;
            }

            if (m_crystalChannel != null)
                m_crystalChannel.onGetActiveCrystalsTotalEnergy += GetActiveCrystalsTotalEnergy;

            if (m_levelChannel.isDebugging)
                DebugSpawn();
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onReset -= CallbackReset;
                m_levelChannel.onPauseGame -= Pause;
                m_levelChannel.onResumeGame -= Resume;
            }

            if (m_crystalChannel != null)
                m_crystalChannel.onGetActiveCrystalsTotalEnergy -= GetActiveCrystalsTotalEnergy;
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// Total remaining energy from all active crystal shards
        /// </summary>
        /// <returns></returns>
        private int GetActiveCrystalsTotalEnergy()
        {
            int totalEnergy = 0;

            foreach (CrystalShard crystal in m_activeCrystals)
                totalEnergy += crystal.remainingEnergyCount;

            return totalEnergy;
        }

        #endregion

        #region Pooling

        /// <summary>
        /// Crystal shards response when leaving the pool
        /// </summary>
        public void GetCrystalFromPool(GameObject gameObject)
        {
            CrystalShard crystal = gameObject.GetComponent<CrystalShard>();
            crystal.Hide();

            crystal.gameObject.SetActive(true);
        }

        /// <summary>
        /// Crystal shards response when re-entering the pool after it's use
        /// </summary>
        public void ReleaseCrystalToPool(CrystalShard crystal)
        {
            if (m_activeCrystals.Contains(crystal))
                m_activeCrystals.Remove(crystal);

            RemoveOccupiedCrystal(crystal);

            m_poolingChannel.onReleaseToPool(crystal.gameObject);
        }

        #endregion

        #region Spawning

        /// <summary>
        /// Spawn a batch of crystal shards inside the arena
        /// </summary>
        /// <param name="config">Crystal shards properties</param>
        public void SpawnCrystalShards(SpawnCrystalShardsConfig config)
        {
            if (!UnityEngine.Application.isPlaying)
                return;

            // Central position for the whole batch of crystal shards
            Vector3 origin = LevelManager.RandomPositionInArena(m_settings.randomPositionExtents);

            Vector3 randomPosition;
            Quaternion randomRotation = UtilsClass.RandomRotation2D();

            for (int i = 0; i < config.count; i++)
            {
                // Get a random position around the central position, and makes sure its not on top of another crystal shard
                randomPosition = GetSafePosition(origin);

                // Pick random amount of energy for one crystal shard
                int energy = UnityEngine.Random.Range(config.minEnergy, config.maxEnergy + 1);

                // Spawn a single crystal shard
                SpawnCrystalShard(randomPosition, randomRotation, energy);
            }

            // Spawn Crystal Edge
            Instantiate(m_crystalsSubjectPrefab, origin, Quaternion.identity, transform);
        }

        private void SpawnCrystalShard(Vector3 randomPosition, Quaternion randomRotation, int energyCount)
        {
            // Pick a random crystal shard prefab 
            GameObject crystalPrefab = m_crystalPoolConfigs.PickRandom().prefab;

            // Get crystal from pool
            CrystalShard crystal = m_poolingChannel.onGetFromPool
                .Invoke(crystalPrefab)
                .GetComponent<CrystalShard>();

            crystal.transform.SetPositionAndRotation(randomPosition, randomRotation);
            crystal.transform.localScale = Vector3.one * m_settings.GetRandomScale();

            crystal.OutOfPool(this, energyCount);
            m_activeCrystals.Add(crystal);
        }

        /// <summary>
        /// Returns a position that's not on top of any other active crystal shard
        /// </summary>
        /// <param name="origin">batch central position</param>
        /// <returns></returns>
        private Vector3 GetSafePosition(Vector3 origin)
        {
            Vector3 position;
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
        /// A position is valid if it's far away from all other active crystal shards
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

        #region Occupied Crystals

        /// <summary>
        /// A turret just landed on this crystal, it's now occupied
        /// </summary>
        public void AddOccupiedCrystal(CrystalShard crystal)
        {
            if (!m_occupiedCrystals.Contains(crystal))
                m_occupiedCrystals.Add(crystal);
        }

        /// <summary>
        /// A turret got retrieved from this crystal, it's no longer occupied
        /// </summary>
        public void RemoveOccupiedCrystal(CrystalShard crystal)
        {
            if (m_occupiedCrystals.Contains(crystal))
                m_occupiedCrystals.Remove(crystal);
        }

        #endregion

        #region Picking Crystals

        public CrystalShard PickRandomOccupiedCrystal(Predicate<CrystalShard> predicateTargetable)
        {
            List<CrystalShard> targetableCrystals = m_occupiedCrystals.FindAll(predicateTargetable);

            if (targetableCrystals.Count == 0)
                return null;
            else
                return targetableCrystals.PickRandom();
        }

        public CrystalShard PickRandomActiveCrystal(Predicate<CrystalShard> predicateTargetable)
        {
            List<CrystalShard> targetableCrystals = m_activeCrystals.FindAll(predicateTargetable);

            if (targetableCrystals.Count == 0)
                return null;
            else
                return targetableCrystals.PickRandom();
        }

        public CrystalShard PickRandomCrystalNearPlayer(float distance, Predicate<CrystalShard> predicateTargetable)
        {
            List<CrystalShard> targetableCrystals = GetCrystalsNearPlayer(distance);
            targetableCrystals = targetableCrystals.FindAll(predicateTargetable);

            if (targetableCrystals.Count == 0)
                return null;
            else
                return targetableCrystals.PickRandom();
        }

        public List<CrystalShard> GetCrystalsNearPlayer(float sqrRange)
        {
            List<CrystalShard> crystals = new List<CrystalShard>();

            Vector3 playerPosition = m_levelChannel.player.transform.position;
            float sqrDistance;

            foreach (CrystalShard crystal in m_activeCrystals)
            {
                sqrDistance = (playerPosition - crystal.transform.position).sqrMagnitude;

                if (sqrDistance < sqrRange)
                    crystals.Add(crystal);
            }

            return crystals;
        }

        #endregion

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

        #region Pause

        public void Pause()
        {
            isPaused = true;
            foreach (CrystalShard crystal in m_activeCrystals)
                crystal.Pause();
        }

        public void Resume()
        {
            isPaused = false;
            foreach (CrystalShard crystal in m_activeCrystals)
                crystal.Resume();
        }



        #endregion

        [ContextMenu("DebugSpawn")]
        public void DebugSpawn()
        {
            SpawnCrystalShards(d_config);
        }


        #endregion
    }
}
