using System;

namespace Bitfrost.Gameplay.Bullets
{

	/// <summary>
	/// Bullet's properties
	/// </summary>
	[Serializable]
	public struct BulletConfig
	{
		public Bullet prefab;

		public float speed;

		public float damage;
	}
}