using Bitfrost.Gameplay.Bullets;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	[CreateAssetMenu(fileName = "ScoutSettings", menuName = "Bitrost/Enemies/Scout Settings", order = 0)]
	public class ScoutSettings : EnemySettings
	{

		[Header("Search Crystal")]
		public float radiusAroundPlayer = 10f;

		/// <summary>
		/// Random angle to position itself around the player
		/// </summary>
		public float angleAroundPlayer;

		/// <summary>
		/// Minimum speed when following player's mouvement
		/// </summary>
		public float minSpeedTrackPlayer;

		/// <summary>
		/// Maximum speed when following player's mouvement
		/// </summary>
		public float maxSpeedTrackPlayer;

		public BulletConfig bulletConfig;

		public float delayBetweenBullet = 4f;
	}
}