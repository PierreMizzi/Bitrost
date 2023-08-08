using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	[CreateAssetMenu(fileName = "EnemyChannel", menuName = "Bitrost/Enemy Channel", order = 1)]
	public class EnemyChannel : ScriptableObject
	{

		[HideInInspector]
		public int killCount;

	}
}