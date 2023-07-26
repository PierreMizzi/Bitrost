using UnityEngine;

[CreateAssetMenu(fileName = "LevelChannel", menuName = "Bitrost/LevelChannel", order = 0)]
public class LevelChannel : ScriptableObject
{
	#region Crystal Shards

    public CrystalShardsManager crystalManager;

	public GameObject player = null;

	#endregion

    private void OnEnable()
    {

    }
}
