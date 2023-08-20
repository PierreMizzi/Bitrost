using System.IO;
using UnityEngine;

namespace PierreMizzi.Useful.SaveSystem
{
	/*
	
		SavedData is very simple : 

		static BaseSaveManager

		static SaveManager

		BaseApplicationData
		ApplicationData : BaseApplicationData
			bestScore : GameData
			{
				Time : 
				KillCount : 
			}

	*/


	public static class BaseSaveManager
	{

		#region Fields 

		private static string saveFolder = "Overcore/Saves/";
		private static string fileName = "ApplicationData";
		private static string fileExtension = ".json";

		private static string directoryPath
		{
			get
			{
				return Application.persistentDataPath + saveFolder;
			}
		}

		private static string path
		{
			get
			{
				return Application.persistentDataPath + saveFolder + fileName + fileExtension;
			}
		}

		private static string dataString;

		public static BaseApplicationData data;

		#endregion

		#region Methods 

		public static void LoadSaveData()
		{
			if (File.Exists(path))
			{
				using StreamReader streamReader = new StreamReader(path);
				dataString = streamReader.ReadToEnd();

				data = JsonUtility.FromJson<BaseApplicationData>(dataString);
			}
			else
				CreateSaveData();
		}

		public static void CreateSaveData()
		{
			Directory.CreateDirectory(directoryPath);

			data = new BaseApplicationData();
			WriteSaveData();
		}

		public static void WriteSaveData()
		{
			dataString = JsonUtility.ToJson(data);

			using StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(dataString);
		}

		public static void LogSaveData()
		{
			string log = "### BASE SAVE MANAGER ###\r\n";
			log += $"path : {path}\r\n";
			log += $"directoryPath : {directoryPath}\r\n";
			log += data.ToString();
			Debug.Log(log);
		}

		#endregion

	}
}