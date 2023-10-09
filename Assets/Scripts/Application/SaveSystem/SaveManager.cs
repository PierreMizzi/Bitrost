using Bitfrost.Gameplay;
using PierreMizzi.Useful.SaveSystem;

namespace Bitfrost.Application
{

	/// <summary>
	/// Tightly related to BaseSaveManager, this class is meant to handle Overcore's own seriliazed data
	/// </summary>
	public static class SaveManager
	{

		public static ApplicationData data;

		public static void LoadSaveData()
		{
			data = BaseSaveManager.LoadSaveData<ApplicationData>();
		}

		public static void WriteSaveData()
		{
			BaseSaveManager.WriteSaveData(data);
		}

		public static void LogSaveData()
		{
			BaseSaveManager.LogBaseSaveManager();
		}

		public static void ManageBestScore(GameOverData gameData)
		{
			if (data.bestScore.totalTime < gameData.totalTime)
				data.bestScore = (GameOverData)gameData.Clone();

			WriteSaveData();
		}


		// public static void (GameOver)
		// {

		// }

	}
}