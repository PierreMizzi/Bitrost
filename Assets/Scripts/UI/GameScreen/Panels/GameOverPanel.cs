using Bitfrost.Application;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful;
using PierreMizzi.Useful.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bitfrost.Gameplay.UI
{

	public class GameOverPanel : APanel
	{

		#region Fields

		[SerializeField]
		protected ApplicationChannel m_applicationChannel = null;

		[SerializeField]
		protected LevelChannel m_levelChannel;

		protected Label m_threatLevel;
		protected Label m_time;
		protected Label m_killCount;

		protected Button m_menuButton;

		private const string k_threatLevel = "time-value";
		private const string k_time = "time-value";
		private const string k_killCount = "score-value";

		private const string k_menuBUtton = "menu-button";

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();

			m_threatLevel = m_root.Q<Label>(k_threatLevel);
			m_time = m_root.Q<Label>(k_time);
			m_killCount = m_root.Q<Label>(k_killCount);

			m_menuButton = m_root.Q<Button>(k_menuBUtton);
			m_menuButton.clicked += CallbackMenuButton;
			m_menuButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
		}

		protected void CallbackOnMouseOver(MouseOverEvent evt)
		{
			SoundManager.PlaySFX(SoundDataID.U_I_HOVER);
		}

		protected void CallbackMenuButton()
		{
			m_applicationChannel.onGameToTitlecard?.Invoke();
			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

		protected virtual void CallbackDisplayPanel(GameOverData data)
		{
			m_threatLevel.text = data.threatLevel.ToString();
			m_time.text = UtilsClass.SecondsToTextTime(data.totalTime);
			m_killCount.text = data.killCount.ToString();

			Display();
		}

		#endregion

	}
}