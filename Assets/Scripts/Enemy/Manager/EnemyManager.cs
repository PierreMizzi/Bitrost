using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*

    Use timeline
    Each tracks manages the spawn rate of each enemies
    Special event for special waves
    Controlable

*/
public partial class EnemyManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private List<EnemySpawningConfig> m_enemySpawnConfigs = new List<EnemySpawningConfig>();

    private List<IEnumerator> m_enemySpawnCoroutines = new List<IEnumerator>();

	#endregion

	#region Methods

    private void Start()
    {
        InitializeSpawning();
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
            Enemy enemy = GetEnemyFromType(config.type);

            float delaySpawn = Random.Range(config.minSpawnDelay, config.maxSpawnDelay);
            yield return new WaitForSeconds(delaySpawn);
        }
    }

	#endregion
}
