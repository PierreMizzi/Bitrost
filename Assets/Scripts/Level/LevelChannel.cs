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

        // Tutorial
        public Action onDisplayTutorial;

        // Timeline stage
        public IntDelegate onChangeStageDifficulty;

        // Player & Turret
        public Player player;
        public IntDelegate onTurretRetrieved;

        // Enemies
        public Action onAllEnemiesKilled;

        // Game Over
        public Action onGameOver;
        public GameOverDelegate onGameOverPanel;

        // Reset Game
        public Action onRestart;
        public Action onReset;

        // Pause
        public Action onPauseGame;
        public Action onResumeGame;

        public Action onDisplayPausePanel;
        public Action onHidePausePanel;

        private void OnEnable()
        {
            // Tutorial
            onDisplayTutorial = () => { };

            // Timeline stage
            onChangeStageDifficulty = (int difficultyLevel) => { };

            // Player & Turret
            onTurretRetrieved = (int storedEnergy) => { };

            // Enemies
            onAllEnemiesKilled = () => { };

            // Game Over
            onGameOver = () => { };
            onGameOverPanel = (GameOverData data) => { };

            // Reset Game
            onReset = () => { };
            onRestart = () => { };

            // Pause
            onPauseGame = () => { };
            onResumeGame = () => { };

            onDisplayPausePanel = () => { };
            onHidePausePanel = () => { };
        }
    }
}
