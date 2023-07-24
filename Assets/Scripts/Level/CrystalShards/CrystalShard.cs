using System;
using UnityEngine;

public class CrystalShard : MonoBehaviour
{
    private CrystalShardsManager m_manager;
    public float quantity { get; private set; }

    public bool isExtracted = false;

    internal void Initialize(CrystalShardsManager manager, int quantity)
    {
        m_manager = manager;
        this.quantity = quantity;
    }
}
