using System;
using System.Collections.Generic;
using PierreMizzi.Useful;
using PierreMizzi.Useful.SceneManagement;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bitfrost.Application
{

	public class TitlecardManager : BaseTitlecardManager, IStateMachine
	{
		#region Fields 

		#region State Machine

		public IStateMachine stateMachine { get { return this; } }
		public AState currentState { get; set; }
		public List<AState> states { get; set; }

		#endregion

		#region Menu

		[Header("Menu")]
		[SerializeField]
		private float m_creditsToMenuDuration;

		public float creditsToMenuDuration
		{
			get { return m_creditsToMenuDuration; }
		}

		private const string k_menu = "menu-container-root";
		private const string k_menuToCreditsButton = "menu-to-credits-button";

		private VisualElement m_menu;
		private Button m_menuToCreditsButton;

		#region Best Score

		private const string k_bestScoreContainer = "best-score-container";
		private const string k_timeLabel = "time-label";
		private const string k_scoreLabel = "score-label";

		private VisualElement m_bestScoreContainer;
		private Label m_timeLabel;
		private Label m_killCountLabel;

		#endregion

		#endregion

		#region Credits

		/*
			Credits
		*/
		[Header("Credits")]
		[SerializeField]
		private float m_menuToCreditsDuration;

		public float menuToCreditsDuration
		{
			get { return m_menuToCreditsDuration; }
		}

		private const string k_credits = "credits-content-root";
		private const string k_creditsToMenuButton = "credits-to-menu-button";

		private VisualElement m_credits;
		private Button m_creditsToMenuButton;

		#endregion

		#endregion

		#region Methods 

		protected override void Awake()
		{
			base.Awake();

			// Menu
			m_menu = m_root.Q<VisualElement>(k_menu);
			m_menuToCreditsButton = m_root.Q<Button>(k_menuToCreditsButton);

			InitializeBestScore();

			// Credits
			m_credits = m_root.Q<VisualElement>(k_credits);
			m_creditsToMenuButton = m_root.Q<Button>(k_creditsToMenuButton);

			m_menuToCreditsButton.clicked += CallbackMenuToCreditsClicked;
			m_creditsToMenuButton.clicked += CallbackCreditsToMenuClicked;

			InitiliazeStates();
		}

		#region State Machine

		public void InitiliazeStates()
		{
			states = new List<AState>()
			{
				new MenuState(this),
				new CreditsState(this),
			};

			ChangeState(TitlecardStateType.Menu, TitlecardStateType.None);
		}

		public void UpdateState() { }

		public void ChangeState(TitlecardStateType nextState, TitlecardStateType previousState = default)
		{
			stateMachine.ChangeState((int)previousState, (int)nextState);
		}

		public void ChangeState(TitlecardStateType nextState)
		{
			stateMachine.ChangeState(currentState.type, (int)nextState);
		}

		#endregion

		#region Menu

		public void DisplayMenu()
		{
			m_menu.RemoveFromClassList(UIToolkitUtils.hide);
		}

		public void HideMenu()
		{
			m_menu.AddToClassList(UIToolkitUtils.hide);
		}

		#region Best Score

		public void InitializeBestScore()
		{
			m_bestScoreContainer = m_root.Q<VisualElement>(k_bestScoreContainer);
			m_timeLabel = m_bestScoreContainer.Q<Label>(k_timeLabel);
			m_killCountLabel = m_bestScoreContainer.Q<Label>(k_scoreLabel);

			RefreshBestScore();
		}

		public void RefreshBestScore()
		{
			m_timeLabel.text = UtilsClass.SecondsToTextTime(SaveManager.data.bestScore.totalTime);
			m_killCountLabel.text = SaveManager.data.bestScore.killCount.ToString();
		}

		#endregion

		#endregion

		#region Credits

		private void CallbackMenuToCreditsClicked()
		{
			if (isInteractable)
				ChangeState(TitlecardStateType.Credits);
		}

		private void CallbackCreditsToMenuClicked()
		{
			if (isInteractable)
				ChangeState(TitlecardStateType.Menu);
		}

		public void DisplayCredits()
		{
			m_credits.RemoveFromClassList(UIToolkitUtils.hide);
		}

		public void HideCredits()
		{
			m_credits.AddToClassList(UIToolkitUtils.hide);
		}




		#endregion

		#endregion
	}
}