namespace Bitfrost.Gameplay.Enemies
{

	public enum EnemyStateType
	{
		None = 0,

		/// <summary>
		/// Inactive, basicaly means inside the pool
		/// </summary>
		Inactive = 1,

		/// <summary>
		/// Enemy is not moving nor attacking
		/// </summary>
		Idle = 2,
		Move = 3,
		Attack = 4,

		/// <summary>
		/// Recently killed and in death animation
		/// </summary>
		Dead = 5,
	}
}