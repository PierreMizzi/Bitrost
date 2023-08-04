using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour
{
    private EnemyManager m_manager = null;

    private EnemySpawnConfig m_config;

    private int currentCount;

    private IEnumerator m_spawnCoroutine;

    public void Initialize(EnemyManager manager)
    {
        m_manager = manager;
    }

    public void ChangeConfig(EnemySpawnConfig config)
    {
        if (Application.isPlaying)
        {
            m_config = config;
            currentCount = config.count;
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (m_spawnCoroutine == null)
        {
            m_spawnCoroutine = SpawningCoroutine();
            StartCoroutine(m_spawnCoroutine);
        }
    }

    public void StopSpawning()
    {
        if (m_spawnCoroutine != null)
        {
            StopCoroutine(m_spawnCoroutine);
            m_spawnCoroutine = null;
        }
    }

    public void PlaySpawning()
    {
        if (m_spawnCoroutine != null)
            StartCoroutine(m_spawnCoroutine);
    }

    public void PauseSpawning()
    {
        if (m_spawnCoroutine != null)
            StopCoroutine(m_spawnCoroutine);
    }

    public IEnumerator SpawningCoroutine()
    {
        while (true)
        {
            m_manager.SpawnEnemy(m_config.prefab.gameObject, m_config.batchCount);
            currentCount -= m_config.batchCount;

            if (currentCount <= 0)
            {
                StopSpawning();
                yield break;
            }
            else
                yield return new WaitForSeconds(m_config.spawnFrequency);
        }
    }

    public void Reset()
    {
        m_config = new EnemySpawnConfig();
        currentCount = 0;
        StopSpawning();
    }
}
