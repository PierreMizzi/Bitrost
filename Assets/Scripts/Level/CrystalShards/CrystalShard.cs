using System;
using UnityEngine;

public class CrystalShard : MonoBehaviour
{
    private CrystalShardsManager m_manager;
    public int totalEnergyCount { get; private set; }

    public int remainingEnergyCount { get; private set; }

    public bool isAvailable = true;

    public void Initialize(CrystalShardsManager manager, int startingEnergyCount)
    {
        m_manager = manager;
        totalEnergyCount = startingEnergyCount;
        remainingEnergyCount = startingEnergyCount;
    }

    public void Extract()
    {
        remainingEnergyCount--;
    }
}
