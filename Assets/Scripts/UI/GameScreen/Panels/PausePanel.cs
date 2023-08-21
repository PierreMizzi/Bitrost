using System;
using Bitfrost.Application;
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
        private ApplicationChannel m_applicationChannel;

        [SerializeField]
        private LevelChannel m_levelChannel;

        private const string k_resumeButton = "resume-button";
        private const string k_menuButton = "menu-button";

        private Button m_resumeButton;
        private Button m_menuButton;

        #endregion

        #region Methods 

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            m_resumeButton = m_root.Q<Button>(k_resumeButton);
            m_menuButton = m_root.Q<Button>(k_menuButton);

            m_resumeButton.clicked += CallbackResumeButton;
            m_menuButton.clicked += CallbackMenuButton;

            m_resumeButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
            m_menuButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
        }

        private void Start()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onDisplayPausePanel += Display;
                m_levelChannel.onHidePausePanel += Hide;
            }
        }

        private void OnDestroy()
        {
            if (m_levelChannel != null)
            {
                m_levelChannel.onDisplayPausePanel -= Display;
                m_levelChannel.onHidePausePanel -= Hide;
            }
        }

        #endregion

        #region Buttons Callbacks

        private void CallbackResumeButton()
        {
            m_levelChannel.onResumeGame.Invoke();
            m_levelChannel.onHidePausePanel.Invoke();
            SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
        }

        private void CallbackMenuButton()
        {
            m_applicationChannel.onGameToTitlecard?.Invoke();
            SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
        }

        private void CallbackOnMouseOver(MouseOverEvent evt)
        {
            SoundManager.PlaySFX(SoundDataID.U_I_HOVER);
        }

        #endregion

        #endregion
    }
}
