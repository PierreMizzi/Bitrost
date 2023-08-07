using System;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.UI;
using UnityEngine;
using UnityEngine.UIElements;


namespace Bitfrost.Gameplay.UI
{
    public class PausePanel : APanel
    {
        #region Fields 

        [SerializeField]
        private LevelChannel m_levelChannel;

        private const string k_resumeButton = "resume-button";
        private const string k_restartButton = "restart-button";
        private const string k_menuButton = "menu-button";

        private Button m_resumeButton;
        private Button m_restartButton;
        private Button m_menuButton;


        #endregion

        #region Methods 

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            m_resumeButton = m_root.Q<Button>(k_resumeButton);
            m_restartButton = m_root.Q<Button>(k_restartButton);
            m_menuButton = m_root.Q<Button>(k_menuButton);

            m_resumeButton.clicked += CallbackResumeButton;
            m_restartButton.clicked += CallbackRestartButton;
            m_menuButton.clicked += CallbackMenuButton;

            m_root.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
        }

        private void Start()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onPauseGame += CallbackPauseGame;
                m_levelChannel.onResumeGame += CallbackResumeGame;
            }
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onPauseGame -= CallbackPauseGame;
                m_levelChannel.onResumeGame -= CallbackResumeGame;
            }
        }

        #endregion

        #region Buttons Callbacks

        private void CallbackResumeButton()
        {
            m_levelChannel.onResumeGame.Invoke();
            SoundManager.PlaySound(SoundDataIDStatic.U_I_CLICK);
        }

        private void CallbackRestartButton()
        {
            Debug.Log("CallbackRestartButton");
            SoundManager.PlaySound(SoundDataIDStatic.U_I_CLICK);
        }

        private void CallbackMenuButton()
        {
            Debug.Log("CallbackMenuButton");
            SoundManager.PlaySound(SoundDataIDStatic.U_I_CLICK);
        }

        private void CallbackOnMouseOver(MouseOverEvent evt)
        {
            VisualElement element = (VisualElement)evt.currentTarget;

            if (element == m_resumeButton ||
                element == m_menuButton ||
                element == m_restartButton
            )
            {
                SoundManager.PlaySound(SoundDataIDStatic.U_I_HOVER);
            }
        }

        #endregion

        private void CallbackPauseGame()
        {
            Debug.Log("CallbackPauseGame");
            Display();
        }

        private void CallbackResumeGame()
        {
            Hide();
        }



        #endregion
    }
}
