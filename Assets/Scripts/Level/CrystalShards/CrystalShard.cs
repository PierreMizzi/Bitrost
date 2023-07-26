using UnityEngine;

public class CrystalShard : MonoBehaviour
{
    private CrystalShardsManager m_manager;

    public float scale { get; private set; }

    public int totalEnergyCount { get; private set; }

    public int remainingEnergyCount { get; private set; }

    public bool isAvailable = true;

    public void Initialize(CrystalShardsManager manager, int startingEnergyCount)
    {
        m_manager = manager;
        totalEnergyCount = startingEnergyCount;
        remainingEnergyCount = startingEnergyCount;
    }

    public void SetUnavailable()
    {
        isAvailable = false;
        m_manager.AddUnavailableCrystal(this);
    }

    public void SetAvailable()
    {
        isAvailable = true;
        m_manager.RemoveUnavailableCrystal(this);
    }

    public void DecrementEnergy()
    {
        remainingEnergyCount--;
    }
}
