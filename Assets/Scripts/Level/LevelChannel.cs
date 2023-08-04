using System;
using UnityEngine;

public delegate void GameOverDelegate(GameOverData data);

[CreateAssetMenu(fileName = "LevelChannel", menuName = "Bitrost/LevelChannel", order = 0)]
public class LevelChannel : ScriptableObject
{
    public CrystalShardsManager crystalManager = null;

    public GameObject player = null;

    public Action onAllEnemiesKilled = null;


    public Action onGameOver;

    public GameOverDelegate onGameOverPanel;

    private void OnEnable()
    {
        onAllEnemiesKilled = () => { };
        onGameOver = () => { };
        onGameOverPanel = (GameOverData data) => { };
    }
}
