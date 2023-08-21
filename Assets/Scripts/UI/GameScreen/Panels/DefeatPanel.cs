using PierreMizzi.SoundManager;
using UnityEngine.UIElements;

namespace Bitfrost.Gameplay.UI
{

	public class DefeatPanel : GameOverPanel
	{

		private Button m_restartButton;
		private const string k_restartButton = "restart-button";

		protected override void Awake()
		{
			base.Awake();

			m_restartButton = m_root.Q<Button>(k_restartButton);
			m_restartButton.clicked += CallbackRestartButton;
			m_restartButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
		}

		private void Start()
		{
			if (m_levelChannel != null)
				m_levelChannel.onDefeatPanel += CallbackDisplayPanel;
		}

		private void OnDestroy()
		{
			if (m_levelChannel != null)
				m_levelChannel.onDefeatPanel -= CallbackDisplayPanel;
		}

		private void CallbackRestartButton()
		{
			m_levelChannel.onRestart?.Invoke();
			Hide();
			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

	}
}