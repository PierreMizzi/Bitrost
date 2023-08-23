using System;
using System.Collections;
using System.Collections.Generic;
using Bitfrost.Application;
using Bitfrost.Gameplay.Enemies;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

namespace Bitfrost.Gameplay
{

    [ExecuteInEditMode]
    public class LevelManager : MonoBehaviour, IPausable
    {
        #region Fields

        [Header("Channels")]
        [SerializeField]
        private ApplicationChannel m_appChannel = null;

        [SerializeField]
        private LevelChannel m_levelChannel = null;

        [SerializeField] private EnemyChannel m_enemyChannel;

        [Header("Arena")]
        [SerializeField]
        private float m_arenaDiameter = 10f;

        public static float arenaRadius;

        public static float arenaRadiusSqr = 0f;

        public bool isPaused { get; set; }

        #region Time

        public float time { get; private set; }

        public bool canUpdateTime { get { return !isPaused; } }

        #endregion

        #region Timeline Stage

        private PlayableDirector m_director;

        private bool isStageCompleted;

        private bool m_isStageDurationOver;

        #endregion

        private int m_currentStageDifficulty;

        [Header("Tutorial")]
        [SerializeField] private bool m_displayTutorial = true;

        [Header("Debug")]
        [SerializeField]
        private bool m_useStartingTime = false;

        [SerializeField]
        private int m_startingTime = 0;

        #endregion

        #region Methods

        #region MonoBehaviour

        private void OnEnable()
        {
            arenaRadius = m_arenaDiameter / 2f;
            arenaRadiusSqr = math.pow(arenaRadius, 2f);
            m_currentStageDifficulty = 0;
        }

        private void Awake()
        {
            m_director = GetComponent<PlayableDirector>();
        }

        private IEnumerator Start()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onChangeStageDifficulty += CallbackChangeStageDifficulty;
                m_levelChannel.onAllEnemiesKilled += CallbackAllEnemiesKilled;
                m_levelChannel.onPlayerDead += CallbackDefeat;
                m_levelChannel.onRestart += CallbackRestart;
                m_levelChannel.onReset += CallbackReset;

                m_levelChannel.onPauseGame += Pause;
                m_levelChannel.onResumeGame += Resume;
            }

            yield return new WaitForSeconds(0.1f);

            if (m_levelChannel.isDebugging)
                ManageDebugMode();
            else
            {
                EnableTimelineStage();
                PlayStage();
            }

            if (m_displayTutorial)
                m_levelChannel.onDisplayTutorial.Invoke();
        }

        private void Update()
        {
            if (canUpdateTime)
                time += Time.unscaledDeltaTime;
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onChangeStageDifficulty -= CallbackChangeStageDifficulty;
                m_levelChannel.onAllEnemiesKilled -= CallbackAllEnemiesKilled;
                m_levelChannel.onPlayerDead -= CallbackDefeat;
                m_levelChannel.onReset -= CallbackReset;
                m_levelChannel.onRestart -= CallbackRestart;

                m_levelChannel.onPauseGame -= Pause;
                m_levelChannel.onResumeGame -= Resume;
            }
        }

        #endregion

        #region Arena

        public static bool IsInsideArena(Vector3 position)
        {
            return position.sqrMagnitude < arenaRadiusSqr;
        }

        public static Vector3 RandomPositionInArena(float edgeRadius)
        {
            return RandomPosition(Vector3.zero, arenaRadius - edgeRadius);
        }

        public static Vector3 RandomPosition(Vector3 origin, float radius)
        {
            float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float randomLength = UnityEngine.Random.Range(0f, 1f);

            return origin + radius * randomLength * new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0);
        }

        public static List<Vector3> RandomPositions(Vector3 origin, int count, float radius)
        {
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < count; i++)
                positions.Add(RandomPosition(origin, radius));


            return positions;
        }

        #endregion

        #region Timeline Stage

        private void PlayStage()
        {
            m_director.Play();
            m_isStageDurationOver = false;
            isStageCompleted = false;
        }

        // Linked to Signal Emitter
        public void CallbackStayStage()
        {
            m_isStageDurationOver = true;

            if (!isStageCompleted)
                m_director.Pause();
            else
                PlayStage();
        }

        // Linked to Signal Emitter
        public void DisplayStageCleared()
        {
            m_levelChannel?.onDisplayStageCleared.Invoke();
        }

        // Linked to Signal Emitter
        public void DisplayHostileDetected()
        {
            m_levelChannel?.onDisplayHostilesDetected.Invoke();
        }

        public void CallbackAllEnemiesKilled()
        {
            isStageCompleted = true;

            if (m_isStageDurationOver)
                PlayStage();
        }

        private void ResetStage()
        {
            m_director.Stop();
            m_director.Play();

            m_currentStageDifficulty = 0;
        }

        private void CallbackChangeStageDifficulty(int value)
        {
            m_currentStageDifficulty = value;
        }

        private void EnableTimelineStage()
        {
            m_director.enabled = true;
            m_displayTutorial = true;
        }

        private void DisableTimelineStage()
        {
            m_director.enabled = false;
            m_displayTutorial = false;
        }

        #endregion

        #region Game Over

        private void CallbackDefeat()
        {
            GameOverData data = new GameOverData(m_currentStageDifficulty, time, m_enemyChannel.killCount);
            SaveManager.ManageBestScore(data);

            m_appChannel.onSetCursor.Invoke(CursorType.Normal);

            m_levelChannel.onPauseGame.Invoke();
            m_levelChannel.onDefeatPanel.Invoke(data);
        }

        // Signal Emitter in Timeline
        public void CallbackVictory()
        {
            GameOverData data = new GameOverData(m_currentStageDifficulty, time, m_enemyChannel.killCount);
            SaveManager.ManageBestScore(data);

            m_appChannel.onSetCursor.Invoke(CursorType.Normal);

            m_levelChannel.onPauseGame.Invoke();
            m_levelChannel.onVictoryPanel.Invoke(data);
        }

        private void CallbackRestart()
        {
            m_levelChannel.onResumeGame.Invoke();
            m_levelChannel.onReset.Invoke();
        }

        private void CallbackReset()
        {
            // Time
            time = 0;

            // Stage
            ResetStage();
        }

        #endregion

        #region Victory

        #endregion

        #region Pause

        public void Pause()
        {
            isPaused = true;
            m_director.Pause();
        }

        public void Resume()
        {
            isPaused = false;
            m_director.Play();
        }

        #endregion

        #region Debug

        private void ManageDebugMode()
        {
            if (m_useStartingTime)
            {
                EnableTimelineStage();
                m_director.time = m_startingTime;
                PlayStage();
            }
            else
                DisableTimelineStage();
        }

        #endregion

        #endregion
    }
}