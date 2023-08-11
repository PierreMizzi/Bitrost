using Bitfrost.Gameplay.Bullets;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	[CreateAssetMenu(fileName = "ScoutSettings", menuName = "Bitrost/Enemies/Scout Settings", order = 0)]
	public class ScoutSettings : EnemySettings
	{

		[Header("Search Crystal")]
		public float radiusAroundPlayer = 10f;
		public float angleAroundPlayer;

		public float minSpeedTrackPlayer;
		public float maxSpeedTrackPlayer;

		public BulletConfig bulletConfig;

		public float delayBetweenBullet = 4f;
	}
}