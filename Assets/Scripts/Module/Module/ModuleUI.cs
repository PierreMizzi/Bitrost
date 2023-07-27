using UnityEngine;
using TMPro;
using System;

public class ModuleUI : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private Module m_module = null;

    private CrystalShard m_crystal = null;

    [SerializeField]
    private TextMeshProUGUI m_crystalText = null;

    [SerializeField]
    private TextMeshProUGUI m_moduleText = null;

    [SerializeField]
    private RectTransform m_extractionImage = null;

    private Vector3 m_extractionImageScale = Vector3.one;

	#endregion

	#region Methods

    private void Start()
    {
        SubscribeModule();
    }

    private void OnDestroy()
    {
        UnsubscribeModule();
    }

    public void Initialize(CrystalShard crystal)
    {
        m_crystal = crystal;
        SubscribeCrystal();
    }

    public void SubscribeModule()
    {
        if (m_module != null)
        {
            m_module.onRefreshModuleEnergy += CallbackRefreshModuleEnergy;
            m_module.onUpdateExtractionUI += CallbackUpdateExtractionUI;
        }
    }

    public void UnsubscribeModule()
    {
        if (m_module != null)
        {
            m_module.onRefreshModuleEnergy -= CallbackRefreshModuleEnergy;
            m_module.onUpdateExtractionUI -= CallbackUpdateExtractionUI;
        }
    }

    public void SubscribeCrystal()
    {
        m_crystal.onRefreshEnergy += CallbackRefreshCrystalEnergy;
    }

    public void UnsubscribeCrystal()
    {
        m_crystal.onRefreshEnergy -= CallbackRefreshCrystalEnergy;
    }

    private void CallbackRefreshCrystalEnergy()
    {
        m_crystalText.text = string.Format(
            "{0} / {1}",
            m_crystal.remainingEnergyCount,
            m_crystal.totalEnergyCount
        );
    }

    // TODO : Refresh when the crystal is being extracted by Harvester
    public void CallbackRefreshModuleEnergy()
    {
        m_moduleText.text = string.Format(
            "{0} / {1}",
            m_module.storedEnergyCount,
            m_module.storedEnergyCapacity
        );
    }

    public void CallbackUpdateExtractionUI(float normalized)
    {
        m_extractionImageScale.x = 1f - normalized;
        m_extractionImage.localScale = m_extractionImageScale;
    }

	#endregion
}
