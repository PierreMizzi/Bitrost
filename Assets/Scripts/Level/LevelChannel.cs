using System;
using Bitfrost.Gameplay.Energy;
using Bitfrost.Gameplay.Players;
using UnityEngine;

namespace Bitfrost.Gameplay
{

    public delegate void GameOverDelegate(GameOverData data);

    [CreateAssetMenu(fileName = "LevelChannel", menuName = "Bitrost/LevelChannel", order = 0)]
    public class LevelChannel : ScriptableObject
    {
        public CrystalShardsManager crystalManager;

        public Player player;
        public IntDelegate onTurretRetrieved;

        public Action onAllEnemiesKilled;

        public Action onDisplayTutorial;

        public Action onGameOver;
        public GameOverDelegate onGameOverPanel;

        public Action onReset;
        public Action onRestart;

        public Action onPauseGame;
        public Action onResumeGame;

        public Action onDisplayPausePanel;
        public Action onHidePausePanel;

        private void OnEnable()
        {
            onTurretRetrieved = (int storedEnergy) => { };

            onAllEnemiesKilled = () => { };

            onDisplayTutorial = () => { };

            // Game Over
            onGameOver = () => { };
            onGameOverPanel = (GameOverData data) => { };

            onReset = () => { };
            onRestart = () => { };

            onPauseGame = () => { };
            onResumeGame = () => { };

            onDisplayPausePanel = () => { };
            onHidePausePanel = () => { };
        }
    }
}
