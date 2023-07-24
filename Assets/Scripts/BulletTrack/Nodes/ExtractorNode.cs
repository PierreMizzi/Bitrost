using TMPro;
using UnityEngine;

public class ExtractorNode : ABulletTrackNode
{
    public TextMeshProUGUI m_quantityText = null;

    private CrystalShard m_crystal = null;

    public void Initialize(CrystalShard crystal)
    {
        m_crystal = crystal;
		RefreshState();
        RefreshUI();
    }

    public void Extract()
    {
        m_crystal.Extract();
        RefreshUI();
    }

    public bool CanExtract()
    {
        return m_crystal.remainingQuantity > 0;
    }

    private void RefreshUI()
    {
        m_quantityText.text = string.Format(
            $"{0} / {1}",
            m_crystal.remainingQuantity,
            m_crystal.quantity
        );
    }
}
