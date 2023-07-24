using System;
using UnityEngine;

public class CrystalShard : MonoBehaviour
{
    private CrystalShardsManager m_manager;
    public float quantity { get; private set; }

    public float remainingQuantity;

    public bool isExtracted = false;

    public void Initialize(CrystalShardsManager manager, int quantity)
    {
        m_manager = manager;
        this.quantity = quantity;
        remainingQuantity = quantity;
    }

    public void Extract()
    {
        remainingQuantity--;
    }
}
