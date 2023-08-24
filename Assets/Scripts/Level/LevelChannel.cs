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

        [SerializeField]
        private bool m_isDebugging = false;
        public bool isDebugging { get { return m_isDebugging; } }

        public CrystalShardsManager crystalManager;

        // Tutorial
        public Action onDisplayTutorialPanel;
        public Action onHideTutorialPanel;

        // Timeline stage
        public IntDelegate onChangeStageDifficulty;

        // Player & Turret
        public Player player;
        public Action onPlayerHit;
        public Action onPlayerDead;
        public IntDelegate onTurretRetrieved;

        // Enemies
        public Action onAllEnemiesKilled;

        // Game Over
        public GameOverDelegate onDefeatPanel;
        public GameOverDelegate onVictoryPanel;

        // PopUp
        public Action onDisplayStageCleared;
        public Action onDisplayHostilesDetected;

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

#if !UNITY_EDITOR

            m_isDebugging = false;

#endif

            // Tutorial
            onDisplayTutorialPanel = () => { };
            onHideTutorialPanel = () => { };

            // Timeline stage
            onChangeStageDifficulty = (int difficultyLevel) => { };

            // Player & Turret
            onTurretRetrieved = (int storedEnergy) => { };

            // Enemies
            onAllEnemiesKilled = () => { };

            // Game Over
            onPlayerDead = () => { };
            onDefeatPanel = (GameOverData data) => { };
            onVictoryPanel = (GameOverData data) => { };

            // PopUp
            onDisplayStageCleared = () => { };
            onDisplayHostilesDetected = () => { };

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
