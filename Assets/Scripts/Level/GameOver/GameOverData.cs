using System;

namespace Bitfrost.Gameplay
{
	[Serializable]
	public struct GameOverData : ICloneable
	{

		public GameOverData(float totalTime, int killCount = 0)
		{
			this.totalTime = totalTime;
			this.killCount = killCount;
		}

		public float totalTime;
		public int killCount;


		public new string ToString()
		{
			string log = "### BEST SCORE :\r\n";
			log += $"totalTime : {totalTime}\r\n";
			log += $"killCount : {killCount}\r\n";
			return log;
		}

		public object Clone()
		{
			return new GameOverData(this.totalTime, this.killCount);
		}
	}
}