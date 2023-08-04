using System;
using UnityEngine;
using UnityEngine.UIElements;

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
    }

    private void CallbackRestartButton()
    {
        Debug.Log("CallbackRestartButton");
    }

    private void CallbackMenuButton()
    {
        Debug.Log("CallbackMenuButton");
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
