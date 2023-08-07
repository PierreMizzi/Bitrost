using System;
using PierreMizzi.Useful.PoolingObjects;

namespace Bitfrost.Gameplay.Enemies
{

	[Serializable]
	public class EnemyPoolConfig : PoolConfig
	{
		public EnemyType type
		{
			get { return prefab.GetComponent<Enemy>().type; }
		}
	}
}