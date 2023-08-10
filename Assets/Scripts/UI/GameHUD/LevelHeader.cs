using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using PierreMizzi.Useful;

namespace Bitfrost.Gameplay.UI
{

    public class LevelHeader : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private LevelChannel m_levelChannel = null;

        [SerializeField]
        private LevelManager m_levelManager;

        private IEnumerator m_updateTimeCoroutine;

        [SerializeField]
        private UIDocument m_document = null;

        private const string k_pauseButton = "pause-button";
        private const string k_timeLabel = "time-label";
        private const string k_difficultyContainer = "difficulty-container";

        private Button m_pauseButton;
        private Label m_timeLabel;
        private VisualElement m_difficultyContainer;

        #endregion

        #region Methods

        private void Awake()
        {
            // Visual Elements
            m_pauseButton = m_document.rootVisualElement.Q<Button>(k_pauseButton);
            m_timeLabel = m_document.rootVisualElement.Q<Label>(k_timeLabel);
            m_difficultyContainer = m_document.rootVisualElement.Q(k_difficultyContainer);

            // Time
            m_updateTimeCoroutine = UpdateTime();
            StartCoroutine(m_updateTimeCoroutine);

            // Pause
            m_pauseButton.clicked += CallbackPauseClicked;
        }

        private void CallbackPauseClicked()
        {
            Debug.LogWarning("Pause Clicked !!!");

            m_levelChannel.onPauseGame.Invoke();
            m_levelChannel.onDisplayPausePanel.Invoke();
        }

        private IEnumerator UpdateTime()
        {
            while (true)
            {
                m_timeLabel.text = UtilsClass.SecondsToTextTime(m_levelManager.time);
                yield return new WaitForSeconds(0.5f);
            }
        }

        #endregion
    }
}
