using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using PierreMizzi.Useful;
using System.Collections.Generic;

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

        private VisualElement m_difficultyIconsContainer;
        List<VisualElement> m_stageDifficultyIcons = new List<VisualElement>();

        #endregion

        #region Methods

        private void Awake()
        {
            // Visual Elements
            m_pauseButton = m_document.rootVisualElement.Q<Button>(k_pauseButton);
            m_timeLabel = m_document.rootVisualElement.Q<Label>(k_timeLabel);

            m_difficultyIconsContainer = m_document.rootVisualElement.Q(k_difficultyContainer);
            m_stageDifficultyIcons = m_difficultyIconsContainer.Children() as List<VisualElement>;
            CallbackChangeStageDifficulty(1);

            // Time
            m_updateTimeCoroutine = UpdateTime();
            StartCoroutine(m_updateTimeCoroutine);

            // Pause
            m_pauseButton.clicked += CallbackPauseClicked;
        }

        private void Start()
        {
            if (m_levelChannel.onChangeStageDifficulty != null)
            {

                m_levelChannel.onReset += CallbackReset;
                m_levelChannel.onChangeStageDifficulty += CallbackChangeStageDifficulty;
            }
        }

        private void OnDestroy()
        {
            if (m_levelChannel.onChangeStageDifficulty != null)
            {
                m_levelChannel.onReset -= CallbackReset;
                m_levelChannel.onChangeStageDifficulty -= CallbackChangeStageDifficulty;
            }
        }

        private void CallbackReset()
        {
            CallbackChangeStageDifficulty(1);
        }

        private void CallbackChangeStageDifficulty(int stageDifficulty)
        {
            int length = m_difficultyIconsContainer.childCount;
            DisplayStyle displayOrHide;
            for (int i = 0; i < length; i++)
            {
                displayOrHide = i < stageDifficulty ? DisplayStyle.Flex : DisplayStyle.None;
                m_stageDifficultyIcons[i].style.display = displayOrHide;
            }
        }

        private void CallbackPauseClicked()
        {
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
