using System;
using Bitfrost.Gameplay;
using PierreMizzi.Useful.SaveSystem;
using UnityEngine;

namespace Bitfrost.Application
{
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
			Debug.Log(data.ToString());
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