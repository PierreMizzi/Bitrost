using System;

namespace Bitfrost.Gameplay
{
	[Serializable]
	public struct GameOverData : ICloneable
	{

		public GameOverData(int threatLevel, float totalTime, int killCount = 0)
		{
			this.threatLevel = threatLevel;
			this.totalTime = totalTime;
			this.killCount = killCount;
		}

		public GameOverData(GameOverData copy)
		{
			this.threatLevel = copy.threatLevel;
			this.totalTime = copy.totalTime;
			this.killCount = copy.killCount;
		}

		public int threatLevel;
		public float totalTime;
		public int killCount;

		public new string ToString()
		{
			string log = "### BEST SCORE :\r\n";
			log += $"threatLevel : {threatLevel}\r\n";
			log += $"totalTime : {totalTime}\r\n";
			log += $"killCount : {killCount}\r\n";
			return log;
		}

		public object Clone()
		{
			return new GameOverData(this);
		}
	}
}