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

		private static string saveFolder = "/Saves/";
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


		#endregion

		#region Methods 

		public static T LoadSaveData<T>() where T : BaseApplicationData, new()
		{
			if (File.Exists(path))
			{
				using StreamReader streamReader = new StreamReader(path);
				string dataString = streamReader.ReadToEnd();

				return JsonUtility.FromJson<T>(dataString);
			}
			else
			{
				Debug.Log("Path doesn't exist");
				CreateSaveData<T>();
				return LoadSaveData<T>();
			}
		}

		public static void CreateSaveData<T>() where T : BaseApplicationData, new()
		{
			Directory.CreateDirectory(directoryPath);

			T data = new T();
			WriteSaveData(data);
		}

		public static void WriteSaveData(BaseApplicationData data)
		{
			string dataString = JsonUtility.ToJson(data);

			using StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(dataString);
		}



		public static void LogBaseSaveManager()
		{
			string log = "### BASE SAVE MANAGER ###\r\n";
			log += $"path : {path}\r\n";
			log += $"directoryPath : {directoryPath}\r\n";
			Debug.Log(log);
		}

		#endregion

	}
}