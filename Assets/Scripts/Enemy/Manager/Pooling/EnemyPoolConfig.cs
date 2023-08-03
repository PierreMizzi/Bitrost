using System;
using UnityEngine;

[Serializable]
public class EnemyPoolConfig : PoolConfig
{
	public EnemyType type
	{
		get { return prefab.GetComponent<Enemy>().type; }
	}
}