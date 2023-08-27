namespace Bitfrost.Gameplay.Energy
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "CrystalShardsChannel", menuName = "Overcore/Crystal Shards Channel", order = 0)]
	public class CrystalShardsChannel : ScriptableObject
	{

		public ReturnIntDelegate onGetActiveCrystalsTotalEnergy;

		public void OnEnable()
		{
			onGetActiveCrystalsTotalEnergy = () => { return 0; };
		}

	}
}