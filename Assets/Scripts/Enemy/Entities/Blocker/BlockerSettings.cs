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
		public float moveRotationDuration;

		[Header("Attack")]

		public float trackingRadius = 1.5f;
		public float trackingSmoothTime = 3;
		public float trackingMaxSpeed = 10;

		[Header("Idle")]
		public float idleOrbitRadius = 3.1f;
		public float idleOrbitSpeed = 10;

		[Header("Weak Spots")]
		public float weakSpotMaxHealth = 10;

	}
}