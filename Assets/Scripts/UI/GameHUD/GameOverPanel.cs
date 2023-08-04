using System;
using CodesmithWorkshop.Useful;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : APanel
{

	#region Fields 

	[SerializeField]
	private LevelChannel m_levelChannel;

	private Label m_timeLabel;
	private Label m_scoreLabel;

	private Button m_restartButton;
	private Button m_menuBUtton;

	private const string k_timeLabel = "time-label";
	private const string k_scoreLabel = "score-label";

	private const string k_restartButton = "restart-button";
	private const string k_menuBUtton = "menu-button";

	#endregion

	#region Methods 

	protected override void Awake()
	{
		base.Awake();

		m_timeLabel = m_root.Q<Label>(k_timeLabel);
		m_scoreLabel = m_root.Q<Label>(k_scoreLabel);
		m_restartButton = m_root.Q<Button>(k_restartButton);
		m_menuBUtton = m_root.Q<Button>(k_menuBUtton);

		m_restartButton.clicked += CallbackRestartButton;
		m_menuBUtton.clicked += CallbackMenuButton;
	}

	private void Start()
	{
		if (m_levelChannel != null)
			m_levelChannel.onGameOverPanel += CallbackGameOverPanel;

	}

	private void OnDestroy()
	{
		if (m_levelChannel != null)
			m_levelChannel.onGameOverPanel -= CallbackGameOverPanel;
	}

	private void CallbackRestartButton()
	{
		Debug.Log("Restart Button");
		m_levelChannel.onReset.Invoke();
		Hide();
	}

	private void CallbackMenuButton()
	{
		Debug.Log("Menu Button");
	}

	private void CallbackGameOverPanel(GameOverData data)
	{
		m_timeLabel.text = UtilsClass.SecondsToTextTime(data.totalTime);
		m_scoreLabel.text = data.totalScore.ToString();

		Display();
	}

	protected override void Display()
	{
		m_root.style.display = DisplayStyle.Flex;
	}

	protected override void Hide()
	{
		m_root.style.display = DisplayStyle.None;
	}



	#endregion

}