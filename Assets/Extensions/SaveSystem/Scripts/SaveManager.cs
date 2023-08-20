using UnityEngine;
using JSONUtility;

namespace PierreMizzi.Useful.SaveSystem
{
	/*
	
		SavedData is very simple : 


		BaseApplicationData

		ApplicationData : BaseApplicationData
			bestScore : GameData
			{
				Time : 
				KillCount : 
			}

	*/


	public static class SaveSystem()
	{

		#region Fields 

		public static string s_saveFolder = "Overcore/Saves"

		public static string path
		{
			get
			{
				return Application.persistentDataPath + s_saveFolder;
			}
		}

		#endregion

		#region Methods 

		#endregion

	}
}