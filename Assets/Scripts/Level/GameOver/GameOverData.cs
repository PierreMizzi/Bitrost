public struct GameOverData
{

	public GameOverData(float totalTime, int totalScore = 0)
	{
		this.totalTime = totalTime;
		this.totalScore = totalScore;
	}

	public float totalTime;
	public int totalScore;
}