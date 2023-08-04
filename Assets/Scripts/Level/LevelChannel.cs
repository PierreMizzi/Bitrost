using System;
using UnityEngine;

public delegate void GameOverDelegate(GameOverData data);

[CreateAssetMenu(fileName = "LevelChannel", menuName = "Bitrost/LevelChannel", order = 0)]
public class LevelChannel : ScriptableObject
{
    public CrystalShardsManager crystalManager = null;

    public Player player = null;

    public Action onAllEnemiesKilled = null;

    public Action onGameOver;
    public GameOverDelegate onGameOverPanel;

    public Action onReset;
    public Action onRestart;

    private void OnEnable()
    {
        onAllEnemiesKilled = () => { };

        // Game Over
        onGameOver = () => { };
        onGameOverPanel = (GameOverData data) => { };

        onReset = () => { };
        onRestart = () => { };
    }
}
