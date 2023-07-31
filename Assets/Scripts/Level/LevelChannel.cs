using UnityEngine;

[CreateAssetMenu(fileName = "LevelChannel", menuName = "Bitrost/LevelChannel", order = 0)]
public class LevelChannel : ScriptableObject
{
    public CrystalShardsManager crystalManager;

    public GameObject player = null;

    private void OnEnable()
    {

    }
}
