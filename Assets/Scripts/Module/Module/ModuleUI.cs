using UnityEngine;
using TMPro;

public class ModuleUI : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private Module m_module = null;

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
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void Subscribe()
    {
        m_module.refreshUI += Refresh;
        m_module.updateExtractionUI += UpdateExtraction;
    }

    public void Unsubscribe()
    {
        m_module.refreshUI -= Refresh;
        m_module.updateExtractionUI -= UpdateExtraction;
    }

    public void Refresh()
    {
        m_crystalText.text = string.Format(
            "{0} / {1}",
            m_module.crystal.remainingEnergyCount,
            m_module.crystal.totalEnergyCount
        );

        m_moduleText.text = string.Format(
            "{0} / {1}",
            m_module.storedEnergyCount,
            m_module.storedEnergyCapacity
        );
    }

    public void UpdateExtraction(float normalized)
    {
        m_extractionImageScale.x = 1f - normalized;
        m_extractionImage.localScale = m_extractionImageScale;
    }

	#endregion
}
