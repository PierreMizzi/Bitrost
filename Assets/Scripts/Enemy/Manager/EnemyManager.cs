using UnityEngine;
using System.Collections.Generic;
using PierreMizzi.Useful.PoolingObjects;

namespace Bitfrost.Gameplay.Enemies
{
    // TODO : Spawn enemy not on a border but on an area -> TO TEST

    [ExecuteInEditMode]
    public class EnemyManager : MonoBehaviour, IPausable
    {

        #region Fields

        [Header("Channels")]
        [SerializeField]
        private LevelChannel m_levelChannel = null;

        [SerializeField]
        private EnemyChannel m_enemyChannel;

        private Camera m_camera = null;

        [Header("Pooling")]
        [SerializeField]
        private PoolingChannel m_poolingChannel = null;

        [SerializeField]
        private List<EnemyPoolConfig> m_enemyPoolConfigs = new List<EnemyPoolConfig>();

        [Header("Debug")]
        [SerializeField]
        private List<EnemySpawnShortcutConfig> m_enemySpawnShortcutConfigs =
            new List<EnemySpawnShortcutConfig>();

        private Dictionary<EnemyType, EnemySpawner> m_enemyTypeToSpawner =
            new Dictionary<EnemyType, EnemySpawner>();

        private List<Enemy> m_activeEnemies = new List<Enemy>();

        #region Spawn Bounds

        [Header("Spawn Bounds")]
        [SerializeField]
        private float m_offsetSpawnBounds = 1f;

        private float m_spawnBoundsAreaSize = 2f;

        private Bounds m_enemySpawnBounds;

        #endregion

        #region Stage Enemy Kill Count

        private int m_stageEnemyCount;

        private int m_stageKilledEnemyCount;

        public bool areAllEnemiesKilled
        {
            get
            {
                return m_stageKilledEnemyCount >= m_stageEnemyCount;
            }
        }

        #endregion

        #region Pause

        public bool isPaused { get; set; }

        #endregion

        #endregion

        #region Methods

        #region MonoBehaviour

        private void Awake()
        {
            m_camera = Camera.main;
            CreateEnemySpawnBounds();
        }

        private void OnEnable()
        {
            m_camera = Camera.main;
            CreateEnemySpawnBounds();
        }

        private void Start()
        {
            m_enemyChannel.killCount = 0;
            InitializeEnemyPools();

            if (m_levelChannel != null)
            {
                m_levelChannel.onReset += CallbackReset;
                m_levelChannel.onPauseGame += Pause;
                m_levelChannel.onResumeGame += Resume;
            }
        }

        private void Update()
        {
            ManageQuickSpawn();
        }

        private void LateUpdate()
        {
            UpdateEnemySpawnBounds();
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onReset -= CallbackReset;
                m_levelChannel.onPauseGame -= Pause;
                m_levelChannel.onResumeGame -= Resume;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(m_enemySpawnBounds.center, m_enemySpawnBounds.size);
            Gizmos.DrawWireCube(
                m_enemySpawnBounds.center,
                m_enemySpawnBounds.size * m_spawnBoundsAreaSize
            );
        }

        #endregion

        #region Spawning

        public void ChangeEnemySpawnConfig(EnemySpawnConfig config)
        {
            if (m_enemyTypeToSpawner.ContainsKey(config.prefab.type))
            {
                m_enemyTypeToSpawner[config.prefab.type].ChangeConfig(config);
                m_stageEnemyCount += config.count;
            }
        }

        public void SpawnEnemy(GameObject prefab, int count)
        {
            for (int i = 0; i < count; i++)
                SpawnEnemy(prefab);
        }

        public void SpawnEnemy(GameObject prefab)
        {
            Enemy enemy = m_poolingChannel.onGetFromPool.Invoke(prefab).GetComponent<Enemy>();
            enemy.transform.position = GetCameraEdgeRandomPosition();
            enemy.OutOfPool(this);

            if (!m_activeEnemies.Contains(enemy))
                m_activeEnemies.Add(enemy);
        }

        private void InitializeEnemyPools()
        {
            foreach (EnemyPoolConfig config in m_enemyPoolConfigs)
            {
                m_poolingChannel.onCreatePool.Invoke(config);
                CreateSpawner(config);
            }
        }

        private void CreateSpawner(EnemyPoolConfig config)
        {
            GameObject newGameObject = new GameObject(config.prefab.name + "Spawner");
            newGameObject.transform.parent = transform;
            EnemySpawner spawner = newGameObject.AddComponent<EnemySpawner>();
            spawner.Initialize(this);

            if (!m_enemyTypeToSpawner.ContainsKey(config.type))
                m_enemyTypeToSpawner.Add(config.type, spawner);
        }




        #endregion

        #region Quick Spawning

        private void ManageQuickSpawn()
        {
            foreach (EnemySpawnShortcutConfig config in m_enemySpawnShortcutConfigs)
            {
                if (Input.GetKeyDown(config.quickSpawnKey))
                    SpawnEnemy(config.prefab.gameObject);
            }
        }

        #endregion

        #region Spawn Bounds

        private void UpdateEnemySpawnBounds()
        {
            Vector3 center = m_camera.transform.position;
            center.z = 0;
            m_enemySpawnBounds.center = center;
        }

        private void CreateEnemySpawnBounds()
        {
            m_enemySpawnBounds = new Bounds();

            // Center
            UpdateEnemySpawnBounds();

            // Size
            Vector3 size = new Vector3();

            size.y = m_camera.orthographicSize * 2f + m_offsetSpawnBounds * 2f;
            size.x = (m_camera.aspect * m_camera.orthographicSize * 2f) + m_offsetSpawnBounds * 2f;

            m_enemySpawnBounds.size = size;
        }

        private Vector3 GetCameraEdgeRandomPosition()
        {
            // Compute random position on the bounds
            Vector3 randomPosition = new Vector2();

            bool horizontalOrVertical = Random.Range(0, 2) == 0;
            bool positiveOrNegative = Random.Range(0, 2) == 0;

            if (horizontalOrVertical)
            {
                randomPosition.x = Random.Range(
                    -m_enemySpawnBounds.extents.x,
                    m_enemySpawnBounds.extents.x
                );
                randomPosition.y = (positiveOrNegative ? 1 : -1) * m_enemySpawnBounds.extents.y;
            }
            else
            {
                randomPosition.y = Random.Range(
                    -m_enemySpawnBounds.extents.y,
                    m_enemySpawnBounds.extents.y
                );
                randomPosition.x = (positiveOrNegative ? 1 : -1) * m_enemySpawnBounds.extents.x;
            }

            Vector3 dirToPosition = randomPosition.normalized;

            randomPosition += m_enemySpawnBounds.center;
            randomPosition += dirToPosition * Random.Range(1f, m_spawnBoundsAreaSize);

            // Compute a new random position inside the area

            return randomPosition;
        }

        #endregion

        #region Killing

        public void KillEnemy(Enemy enemy)
        {
            m_poolingChannel.onReleaseFromPool.Invoke(enemy.gameObject);

            if (m_activeEnemies.Contains(enemy))
                m_activeEnemies.Remove(enemy);

            m_stageKilledEnemyCount += 1;

            m_enemyChannel.killCount++;

            if (areAllEnemiesKilled)
                m_levelChannel.onAllEnemiesKilled.Invoke();
        }

        #endregion

        #region Reset

        public void CallbackReset()
        {

            m_enemyChannel.killCount = 0;

            ResetEnemySpawner();

            ResetActiveEnemies();
        }

        public void ResetEnemySpawner()
        {
            foreach (KeyValuePair<EnemyType, EnemySpawner> pair in m_enemyTypeToSpawner)
                pair.Value.Reset();
        }

        public void ResetActiveEnemies()
        {
            foreach (Enemy enemy in m_activeEnemies)
            {
                enemy.ChangeState(EnemyStateType.Inactive);
                m_poolingChannel.onReleaseFromPool.Invoke(enemy.gameObject);
            }

            m_activeEnemies.Clear();
        }

        #endregion

        #region Pause


        public void Pause()
        {
            isPaused = true;

            PauseActiveEnemies();
            PauseEnemySpawners();
        }

        private void PauseEnemySpawners()
        {
            foreach (KeyValuePair<EnemyType, EnemySpawner> pair in m_enemyTypeToSpawner)
                pair.Value.Pause();
        }

        private void PauseActiveEnemies()
        {
            foreach (Enemy enemy in m_activeEnemies)
                enemy.Pause();
        }

        public void Resume()
        {
            isPaused = false;

            ResumeActiveEnemies();
            ResumeEnemySpawners();
        }

        private void ResumeEnemySpawners()
        {
            foreach (KeyValuePair<EnemyType, EnemySpawner> pair in m_enemyTypeToSpawner)
                pair.Value.Resume();
        }

        private void ResumeActiveEnemies()
        {
            foreach (Enemy enemy in m_activeEnemies)
                enemy.Resume();
        }

        #endregion

        #endregion

    }
}
