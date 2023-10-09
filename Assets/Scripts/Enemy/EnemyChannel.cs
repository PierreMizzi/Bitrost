using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	[CreateAssetMenu(fileName = "EnemyChannel", menuName = "Overcore/Channels/Gameplay/Enemy Channel", order = 1)]
	public class EnemyChannel : ScriptableObject
	{

		[HideInInspector]
		public int killCount;

		public ReturnFloatDelegate onGetActiveEnemiesTotalHealth;

		public void OnEnable()
		{
			onGetActiveEnemiesTotalHealth = () => { return 0f; };
		}

	}
}