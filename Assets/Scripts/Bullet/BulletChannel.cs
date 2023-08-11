using System;
using System.Collections.Generic;
using PierreMizzi.Useful.PoolingObjects;
using UnityEngine;

namespace Bitfrost.Gameplay.Bullets
{

	public delegate void FireBulletDelegate(
		BulletConfig config,
		IBulletLauncher launcher,
		Vector3 position,
		Vector3 orientation
	);

	public delegate void ReleaseBulletDelegate(Bullet bullet);

	public delegate void DisplayImpactDelegate(BulletImpact impact, Vector3 position);

	public delegate void ReleaseImpactDelegate(BulletImpact impact);


	[CreateAssetMenu(fileName = "BulletChannel", menuName = "Bitrost/Bullet/BulletChannel", order = 0)]
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
