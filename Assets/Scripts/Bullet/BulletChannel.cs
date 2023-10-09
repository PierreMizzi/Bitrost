using System;
using System.Collections.Generic;
using PierreMizzi.Useful.PoolingObjects;
using UnityEngine;

namespace Bitfrost.Gameplay.Bullets
{
	/// <summary>
	/// Delegate called when firing a bullet
	/// </summary>
	/// <param name="config">Bullet's properties (speed, damage etc)</param>
	/// <param name="launcher">Entity who fired the bullet</param>
	/// <param name="position">Starting position in world space</param>
	/// <param name="orientation">Starting orientation (up vector)</param>
	public delegate void FireBulletDelegate(
		BulletConfig config,
		IBulletLauncher launcher,
		Vector3 position,
		Vector3 orientation
	);

	public delegate void ReleaseBulletDelegate(Bullet bullet);

	public delegate void DisplayImpactDelegate(BulletImpact impact, Vector3 position);

	public delegate void ReleaseImpactDelegate(BulletImpact impact);

	/// <summary>
	/// Handles bullets and bullet impacts related events 
	/// </summary>
	[CreateAssetMenu(fileName = "BulletChannel", menuName = "Overcore/Channels/Gameplay/Bullet Channel", order = 0)]
	public class BulletChannel : ScriptableObject
	{
		[Header("Bullets")]
		public List<PoolConfig> bulletPoolConfigs = new List<PoolConfig>();

		public FireBulletDelegate onFireBullet;

		public ReleaseBulletDelegate onReleaseBullet;

		[Header("Bullet Impacts")]
		public List<PoolConfig> impactPoolConfigs = new List<PoolConfig>();

		public DisplayImpactDelegate onDisplayImpact = null;

		public ReleaseImpactDelegate onReleaseImpact;

		private void OnEnable()
		{
			// Bullets
			onFireBullet = (
				BulletConfig config,
				IBulletLauncher launcher,
				Vector3 position,
				Vector3 orientation
			) =>
			{ };

			onReleaseBullet = (Bullet bullet) => { };

			// Impacts
			onDisplayImpact = (BulletImpact impact, Vector3 position) => { };
			onReleaseImpact = (BulletImpact impact) => { };
		}
	}
}
