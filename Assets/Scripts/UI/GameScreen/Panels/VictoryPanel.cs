namespace Bitfrost.Gameplay.UI
{

	/*
	
		- Event Signal in Timeline
		- event in LevelManager
		-> Callback
			- Display

		
		VictoryPanel :
		- "Bravo !"
		- KillCount
		- Restart ?
		- Menu

	*/
	public class VictoryPanel : GameOverPanel
	{

		private void Start()
		{
			if (m_levelChannel != null)
				m_levelChannel.onVictoryPanel += CallbackDisplayPanel;
		}

		private void OnDestroy()
		{
			if (m_levelChannel != null)
				m_levelChannel.onVictoryPanel -= CallbackDisplayPanel;
		}

	}
}