using UnityEngine;
using PierreMizzi.Useful.UI;

namespace Bitfrost.Gameplay.UI
{

	public class HostilesDetectedPopUp : PopUp
	{

		[SerializeField]
		private LevelChannel m_levelChannel;

		protected virtual void Start()
		{
			if (m_levelChannel != null)
				m_levelChannel.onDisplayHostilesDetected += DisplayThenHide;
		}

		protected virtual void OnDestroy()
		{
			if (m_levelChannel != null)
				m_levelChannel.onDisplayHostilesDetected -= DisplayThenHide;
		}

	}
}