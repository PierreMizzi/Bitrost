using System;
using PierreMizzi.Useful.PoolingObjects;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	[Serializable]
	public class EnemyPoolConfig : PoolConfig
	{
		public EnemyType type
		{
			get { return prefab.GetComponent<Enemy>().type; }
		}

		[Header("Debug")]
		public KeyCode quickSpawnKey;

	}
}