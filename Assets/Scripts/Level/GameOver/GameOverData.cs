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
	}
}