using System;
using Bitfrost.Gameplay.Enemies;
using Bitfrost.Gameplay.Energy;
using Bitfrost.Gameplay.Players;
using Bitfrost.Gameplay.Turrets;
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

        // TODO Use CrystalShardsManager instead
        public CrystalShardsManager crystalManager;

        // Tutorial
        public Action onDisplayTutorialPanel;
        public Action onTutorialStartClicked;

        // Timeline stage
        public IntDelegate onChangeStageDifficulty;

        // Player & Turret
        public Player player;
        public Action onPlayerHit;
        public IntDelegate onTurretRetrieved;

        // Loosing Conditions
        [Obsolete]
        public Action onPlayerDead;
        // Player has no health
        //      Can't act (move + turret)
        // Player's explosion animation has ended
        //      GameOver
        //      DefeatPanel

        /*
            No Health

            -onDisablePlayer
            Player's animation explosion
            - onGameOver (onPlayerDead)
            - onDefeatPanel

            Insufficent

            - onInsufficientEnergy
            - onDisablePlayer
            InsufficentEnergy animation
            - GameOver
            - onDefeatPanel

        */
        public Action onDisablePlayerControls;

        /// <summary> 
        /// Can be done through reset
        /// </summary>
        [Obsolete]
        public Action onEnablePlayerControls;
        public Action onGameOver;


        // Enemies
        public Action onAllEnemiesKilled;

        // Game Over
        public GameOverDelegate onDefeatPanel;
        public GameOverDelegate onVictoryPanel;

        // PopUp
        public Action onDisplayStageCleared;
        public Action onDisplayHostilesDetected;
        public Action onInsufficientEnergy;


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
            onTutorialStartClicked = () => { };

            // Timeline stage
            onChangeStageDifficulty = (int difficultyLevel) => { };

            // Player & Turret
            onTurretRetrieved = (int storedEnergy) => { };

            // Enemies
            onAllEnemiesKilled = () => { };

            // Game Over
            onPlayerDead = () => { };
            onInsufficientEnergy = () => { };
            onDefeatPanel = (GameOverData data) => { };
            onVictoryPanel = (GameOverData data) => { };

            onGameOver = () => { };
            onDisablePlayerControls = () => { };
            onEnablePlayerControls = () => { };

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
