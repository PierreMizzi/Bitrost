using System;
using System.Collections;
using System.Collections.Generic;
using PierreMizzi.SoundManager;
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

		private const string k_versionLabel = "version-label";
		private const string k_menu = "menu-container-root";
		private const string k_menuToCreditsButton = "menu-to-credits-button";

		private Label m_versionLabel;
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

		[Header("Camera")]
		[SerializeField]
		private TitlecardCameraController m_cameraController;
		public TitlecardCameraController cameraController => m_cameraController;

		[Header("Music")]
		[SerializeField]
		private SoundSource m_musicSoundSource;

		#endregion

		#region Methods 

		protected override void Awake()
		{
			base.Awake();

			// Menu
			m_menu = m_root.Q<VisualElement>(k_menu);
			m_menuToCreditsButton = m_menu.Q<Button>(k_menuToCreditsButton);
			m_menuToCreditsButton.clicked += CallbackMenuToCreditsClicked;
			m_menuToCreditsButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);

			m_startButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
			m_exitButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);

			InitializeBestScore();

			// Credits
			m_credits = m_root.Q<VisualElement>(k_credits);

			m_versionLabel = m_credits.Q<Label>(k_versionLabel);
			m_versionLabel.text = "v" + UnityEngine.Application.version;

			m_creditsToMenuButton = m_credits.Q<Button>(k_creditsToMenuButton);
			m_creditsToMenuButton.clicked += CallbackCreditsToMenuClicked;
			m_creditsToMenuButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);

			// State Machine
			InitiliazeStates();
		}

		protected override IEnumerator UnloadTitlecardSceneCoroutine()
		{
			yield return base.UnloadTitlecardSceneCoroutine();
			m_musicSoundSource.FadeOut();
			yield return null;
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

		protected override void CallbackStartClicked()
		{
			base.CallbackStartClicked();
			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

		protected override void CallbackExitClicked()
		{
			base.CallbackExitClicked();
			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

		private void CallbackOnMouseOver(MouseOverEvent evt)
		{
			SoundManager.PlaySFX(SoundDataID.U_I_HOVER);
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

			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

		private void CallbackCreditsToMenuClicked()
		{
			if (isInteractable)
				ChangeState(TitlecardStateType.Menu);

			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
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