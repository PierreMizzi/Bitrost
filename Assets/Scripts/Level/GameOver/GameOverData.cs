namespace Bitfrost.Gameplay
{
	public struct GameOverData
	{

		public GameOverData(float totalTime, int totalScore = 0)
		{
			this.totalTime = totalTime;
			this.killCount = totalScore;
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
	}
}