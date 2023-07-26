using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*

    Use timeline
    Each tracks manages the spawn rate of each enemies
    Special event for special waves
    Controlable

*/

[ExecuteInEditMode]
public partial class EnemyManager : MonoBehaviour
{
	#region Fields

    private Camera m_camera = null;

    [SerializeField]
    private PoolingChannel m_poolingChannel = null;

    [SerializeField]
    private List<EnemyPoolConfig> m_enemyPoolConfigs = new List<EnemyPoolConfig>();

    [SerializeField]
    private List<EnemySpawningConfig> m_enemySpawnConfigs = new List<EnemySpawningConfig>();

    private List<IEnumerator> m_enemySpawnCoroutines = new List<IEnumerator>();

	#endregion

	#region Methods
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
        InitializeEnemyPools();
        InitializeSpawning();
    }

    private void LateUpdate()
    {
        UpdateEnemySpawnBounds();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(m_enemySpawnBounds.center, m_enemySpawnBounds.size);
    }

    private void InitializeSpawning()
    {
        foreach (EnemySpawningConfig config in m_enemySpawnConfigs)
        {
            IEnumerator coroutine = SpawningCoroutine(config);
            StartCoroutine(coroutine);

            m_enemySpawnCoroutines.Add(coroutine);
        }
    }

    private IEnumerator SpawningCoroutine(EnemySpawningConfig config)
    {
        while (true)
        {
            Enemy enemy = m_poolingChannel.onGetFromPool
                .Invoke(config.prefab.gameObject)
                .GetComponent<Enemy>();

            enemy.transform.position = GetCameraEdgeRandomPosition();

            float delaySpawn = Random.Range(config.minSpawnDelay, config.maxSpawnDelay);
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    private void InitializeEnemyPools()
    {
        foreach (EnemyPoolConfig config in m_enemyPoolConfigs)
        {
            m_poolingChannel.onCreatePool.Invoke(config);
        }
    }

    private Bounds m_enemySpawnBounds;

    [SerializeField]
    private float m_offsetSpawnBounds = 1f;

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

        randomPosition += m_enemySpawnBounds.center;
        return randomPosition;
    }

	#endregion
}
