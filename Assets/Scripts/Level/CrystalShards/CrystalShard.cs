using System;
using UnityEngine;
using PierreMizzi.Rendering;

public class CrystalShard : MonoBehaviour
{
    private CrystalShardsManager m_manager;

    public float scale { get; private set; }

    public int totalEnergyCount { get; private set; }

    public int remainingEnergyCount { get; private set; }

    public float energyPercentage
    {
        get { return remainingEnergyCount / (float)totalEnergyCount; }
    }

    public bool hasEnergy
    {
        get { return remainingEnergyCount > 0; }
    }

    public Action onRefreshEnergy = null;

    public bool isAvailable = true;

    #region Rendering

    [SerializeField]
    private MaterialPropertyBlockModifier m_propertyBlock = null;

    private const string k_energyPercentProperty = "_EnergyPercent";
    private const string k_noiseOffsetProperty = "_NoiseOffset";

    #endregion

    private void Awake()
    {
        onRefreshEnergy = () => { };
    }

    [ContextMenu("Start")]
    private void Start()
    {
        Vector4 noiseOffset = new Vector4();
        noiseOffset.x = UnityEngine.Random.Range(0f, 100f);
        noiseOffset.y = UnityEngine.Random.Range(0f, 100f);
        m_propertyBlock.SetProperty(k_noiseOffsetProperty, noiseOffset);
    }

    public void Destroy()
    {
        Reset();
        m_manager.DestroyCrystal(this);
    }

    public void Initialize(CrystalShardsManager manager, int startingEnergyCount)
    {
        m_manager = manager;
        totalEnergyCount = startingEnergyCount;
        remainingEnergyCount = startingEnergyCount;
        SetVisualEnergy();
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
        SetVisualEnergy();
        onRefreshEnergy.Invoke();
    }

    private void SetVisualEnergy()
    {
        m_propertyBlock.SetProperty(k_energyPercentProperty, energyPercentage);
    }

    public void Reset()
    {
        remainingEnergyCount = 0;
        totalEnergyCount = 0;
        isAvailable = true;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
