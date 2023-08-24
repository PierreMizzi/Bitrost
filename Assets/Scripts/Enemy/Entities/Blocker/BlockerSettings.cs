using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	[CreateAssetMenu(
		fileName = "FighterSettings",
		menuName = "Bitrost/Enemies/BlcokerSettings",
		order = 0
	)]
	public class BlockerSettings : EnemySettings
	{

		[Header("Movement")]
		public float trackingRotationSpeed;

		[Header("Weak Spots")]
		public float weakSpotMaxHealth = 10;

	}
}