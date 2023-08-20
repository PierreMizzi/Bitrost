using PierreMizzi.Useful.SaveSystem;

namespace Bitfrost.Application
{
	public static class SaveManager
	{

		public static ApplicationData data
		{
			get
			{
				return BaseSaveManager.data as ApplicationData;
			}
		}

		public static void LoadSaveData()
		{
			BaseSaveManager.LoadSaveData();
		}

		public static void WriteSaveData()
		{
			BaseSaveManager.data = data;
			BaseSaveManager.WriteSaveData();
		}

		public static void LogSaveData()
		{
			BaseSaveManager.LogSaveData();
		}

	}
}