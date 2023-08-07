using System;

namespace Bitfrost.Gameplay.Bullets
{

	[Serializable]
	public struct BulletConfig
	{
		public Bullet prefab;

		public float speed;

		public float damage;
	}
}