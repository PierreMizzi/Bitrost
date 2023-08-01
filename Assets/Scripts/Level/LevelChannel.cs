using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelChannel", menuName = "Bitrost/LevelChannel", order = 0)]
public class LevelChannel : ScriptableObject
{
    public CrystalShardsManager crystalManager = null;

    public GameObject player = null;

    public Action onAllEnemiesKilled = null;

    private void OnEnable()
    {
        onAllEnemiesKilled = () => { };
    }
}
