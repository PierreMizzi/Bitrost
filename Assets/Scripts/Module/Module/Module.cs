using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Collections;

public class Module : MonoBehaviour, IBulletLauncher
{
    #region Fields

    public CrystalShard crystal { get; private set; }

    [SerializeField]
    private ModuleSettings m_settings;

    [SerializeField]
    private BulletChannel m_bulletChannel;

    private ModuleManager m_manager;

    [SerializeField]
    private Transform m_turret;

    [SerializeField]
    private Transform m_bulletOrigin;

    private Vector3 m_aimDirection = new Vector3();

    private PlayerController m_player;

    #region UI

    [SerializeField]
    private ModuleUI m_ui = null;

    public Action onRefreshModuleEnergy = null;

    public FloatDelegate onUpdateExtractionUI = null;

    #endregion

    #region Extracting

    private bool m_isExtracting = false;

    public float extractionNormalized { get; private set; }

    public int storedEnergyCount { get; private set; }

    public int storedEnergyCapacity { get; private set; }

    #endregion

    #endregion

    #region Methods

    public void Initialize(ModuleManager manager, CrystalShard crystal)
    {
        m_manager = manager;

        this.crystal = crystal;
        this.crystal.SetUnavailable();
        m_ui.Initialize(this.crystal);

        storedEnergyCount = 0;
        storedEnergyCapacity = m_settings.storedEnergyCapacity;
    }

    #region MonoBehaviour

    protected void Awake()
    {
        m_player = FindObjectOfType<PlayerController>();

        onRefreshModuleEnergy = () => { };
        onUpdateExtractionUI = (float normalized) => { };
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        onRefreshModuleEnergy.Invoke();
        crystal.onRefreshEnergy.Invoke();

        yield return null;
    }

    public void Update()
    {
        UpdateRotation();
    }

    #endregion

    #region UI

    #endregion

    #region Extract

    public void Extract()
    {
        if (CanExtract())
        {
            m_isExtracting = true;
            extractionNormalized = 0;

            DOVirtual
                .Float(0, 1, m_settings.extractionDuration, ExtractUpdate)
                .SetEase(Ease.Linear)
                .OnComplete(CompleteExtract);
        }
    }

    public void ExtractUpdate(float value)
    {
        extractionNormalized = value;
        onUpdateExtractionUI.Invoke(extractionNormalized);
    }

    public void CompleteExtract()
    {
        m_isExtracting = false;
        crystal.DecrementEnergy();
        storedEnergyCount += 2;

        if (storedEnergyCount > storedEnergyCapacity)
            storedEnergyCount = storedEnergyCapacity;

        extractionNormalized = 0;
        onUpdateExtractionUI.Invoke(1);
        onRefreshModuleEnergy.Invoke();
    }

    public bool CanExtract()
    {
        bool result = true;

        // Stored energy is full
        bool canStoreEnergy = storedEnergyCount < storedEnergyCapacity;
        result &= canStoreEnergy;
        if (!canStoreEnergy)
            Debug.LogWarning("CAN'T STORE ENERGY");

        // Is already Extracting ?
        result &= !m_isExtracting;
        if (m_isExtracting)
            Debug.LogWarning("IS EXTRACTING");

        // Crystal has energy ?
        bool crystalHasEnergy = crystal.remainingEnergyCount > 0;
        result &= crystalHasEnergy;
        if (!crystalHasEnergy)
            Debug.LogWarning("CRYSTAL IS DEPLEATED");

        return result;
    }

    #endregion

    #region Retrieve

    public void Retrieve()
    {
        crystal.SetAvailable();
        m_ui.UnsubscribeCrystal();

        if(crystal.remainingEnergyCount == 0)
            Destroy(crystal.gameObject);
    }

    #endregion

    #region Fire

    public void Fire()
    {
        if (CanFire())
        {
            if (storedEnergyCount > 0)
            {
                storedEnergyCount--;
                onRefreshModuleEnergy.Invoke();
            }
            else
                crystal.DecrementEnergy();

            m_bulletChannel.onInstantiateBullet.Invoke(
                this,
                m_settings.bulletPrefab,
                m_bulletOrigin.position,
                m_aimDirection
            );
        }
    }

    public bool CanFire()
    {
        bool result = true;

        // Is extracting ?
        result &= !m_isExtracting;
        if (m_isExtracting)
            Debug.LogWarning("IS EXTRACTING");

        // Has energy in crystal or stored ?
        bool hasEnergy = crystal.remainingEnergyCount > 0 || storedEnergyCount > 0;
        result &= hasEnergy;
        if (!hasEnergy)
            Debug.LogWarning("NO ENERGY");

        return result;
    }

    private void UpdateRotation()
    {
        m_aimDirection = (m_player.transform.position - transform.position).normalized;
        m_turret.up = m_aimDirection;
    }

    #endregion

    #endregion
}
