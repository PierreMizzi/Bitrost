using PierreMizzi.Useful.UI;
using UnityEngine;

namespace Bitfrost.Gameplay.UI
{

	public class InsufficientEnergyPopUp : PopUp
	{
		[SerializeField]
		private LevelChannel m_levelChannel;

		protected virtual void Start()
		{
			if (m_levelChannel != null)
				m_levelChannel.onInsufficientEnergy += DisplayThenHide;
		}

		protected virtual void OnDestroy()
		{
			if (m_levelChannel != null)
				m_levelChannel.onInsufficientEnergy -= DisplayThenHide;
		}

		protected override void DisplayThenHideEnded()
		{
			m_levelChannel?.onGameOver.Invoke();
		}
	}
}