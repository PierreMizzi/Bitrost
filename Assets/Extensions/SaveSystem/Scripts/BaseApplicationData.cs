using System;
using UnityEngine;

namespace PierreMizzi.Useful.SaveSystem
{

	[Serializable]
	public class BaseApplicationData
	{
		public BaseApplicationData()
		{
			lastSaveDateTime = DateTime.Now;
			lastSaveTime = lastSaveDateTime.ToString();
		}


		public string lastSaveTime;
		public DateTime lastSaveDateTime;

		public new virtual string ToString()
		{
			string log = "### SAVE DATA ###\r\n";
			log += $"lastSaveTime : {lastSaveTime}\r\n";
			log += $"lastSaveDateTime : {lastSaveDateTime}\r\n";
			return log;
		}
	}

}