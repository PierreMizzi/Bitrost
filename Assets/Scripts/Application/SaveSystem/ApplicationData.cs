using System;
using PierreMizzi.Useful.SaveSystem;
using Bitfrost.Gameplay;

namespace Bitfrost.Application
{

	[Serializable]
	public class ApplicationData : BaseApplicationData
	{

		public GameOverData bestScore;

		public override string ToString()
		{
			string log = base.ToString() + bestScore.ToString();
			log += "#########";
			return log;
		}

	}
}