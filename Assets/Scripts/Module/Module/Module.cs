using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public delegate void ModuleDelegate(Module module);

// TODO : back to Fire Mode when retrieving while extracting
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

    [SerializeField]
    private InputActionReference m_mousePositionActionReference = null;

    private Vector3 m_mouseWorldSpace;

    private Camera m_camera;

    #region UI

    [SerializeField]
    private ModuleUI m_ui = null;

    public Action onRefreshEnergy = null;

    public FloatDelegate onUpdateExtractionUI = null;

    #endregion



    #region Extracting

    public bool isExtracting { get; private set; }

    public float extractionNormalized { get; private set; }

    public int storedEnergyCount { get; private set; }

    public int storedEnergyCapacity { get; private set; }

    public Tween m_extractingTween = null;

    #endregion

    #region Reworked UI

    [SerializeField]
    private GameObject m_spritesContainer = null;

    // Activation
    [HideInInspector]
    public bool isActive = false;

    public Action onActivation = null;

    public Action onAssignCrystal = null;

    public Action onRemoveCrystal = null;

    public Action onExtraction = null;

    public Action onStoredEnergyAvailable = null;
    public Action onCrystalEnergyAvailable = null;

    // [SerializeField]
    // private ModuleView m_view = null;

    #endregion

    #endregion

    #region Methods

    [Obsolete]
    public void Initialize(ModuleManager manager, CrystalShard crystal)
    {
        m_manager = manager;

        this.crystal = crystal;
        this.crystal.SetUnavailable();
        m_ui.Initialize(this.crystal);

        storedEnergyCount = 0;
        storedEnergyCapacity = m_settings.storedEnergyCapacity;
    }

    public void Initialize(ModuleManager manager)
    {
        m_manager = manager;

        storedEnergyCount = 0;

        SetInactive();
    }

    #region MonoBehaviour

    protected void Awake()
    {
        m_camera = Camera.main;
        m_player = FindObjectOfType<PlayerController>();
        storedEnergyCapacity = m_settings.storedEnergyCapacity;

        onActivation = () => { };
        onAssignCrystal = () => { };
        onRemoveCrystal = () => { };

        onRefreshEnergy = () => { };
        onUpdateExtractionUI = (float normalized) => { };

        onStoredEnergyAvailable = () => { };
        onCrystalEnergyAvailable = () => { };
    }

    public void Update()
    {
        if (isActive)
            UpdateRotation();
    }

    #endregion

    #region Activation

    public void SetActive()
    {
        isActive = true;
        m_spritesContainer.SetActive(true);

        transform.position = this.crystal.transform.position;

        onActivation.Invoke();
        onRefreshEnergy.Invoke();
        onExtraction.Invoke();
        onCrystalEnergyAvailable.Invoke();
    }

    public void SetInactive()
    {
        isActive = false;
        m_spritesContainer.SetActive(false);

        onActivation.Invoke();

        if (m_extractingTween != null && m_extractingTween.IsPlaying())
            m_extractingTween.Kill();
    }

    public void AssignCrystal(CrystalShard crystal)
    {
        this.crystal = crystal;
        this.crystal.SetUnavailable();

        onAssignCrystal.Invoke();
        this.crystal.onRefreshEnergy.Invoke();
    }

    public void RemoveCrystal()
    {
        crystal.SetAvailable();

        if (!crystal.hasEnergy)
            Destroy(crystal.gameObject);

        onRemoveCrystal.Invoke();
        crystal = null;
    }

    #endregion

    #region Extract

    public void ToggleExtracting()
    {
        if (!isExtracting)
            StartExtracting();
        else
            StopExtracting();
    }

    private void StartExtracting()
    {
        if (CanExtract())
        {
            isExtracting = true;
            extractionNormalized = 0;

            m_extractingTween = DOVirtual
                .Float(0, 1, m_settings.extractionDuration, ExtractingUpdate)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .OnStepComplete(CompleteExtracting);

            onExtraction.Invoke();
        }
    }

    private void StopExtracting()
    {
        m_extractingTween.Kill();
        isExtracting = false;
        extractionNormalized = 0;
        onUpdateExtractionUI.Invoke(1);
        onExtraction.Invoke();
    }

    public void ExtractingUpdate(float value)
    {
        extractionNormalized = value;
        onUpdateExtractionUI.Invoke(extractionNormalized);
    }

    public void CompleteExtracting()
    {
        crystal.DecrementEnergy();

        if (!crystal.hasEnergy)
            StopExtracting();

        storedEnergyCount += 2;

        if (storedEnergyCount > storedEnergyCapacity)
        {
            storedEnergyCount = storedEnergyCapacity;
            StopExtracting();
        }

        extractionNormalized = 0;
        onUpdateExtractionUI.Invoke(1);
        onRefreshEnergy.Invoke();
        onStoredEnergyAvailable.Invoke();
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
        result &= !isExtracting;
        if (isExtracting)
            Debug.LogWarning("IS EXTRACTING");

        // Crystal has energy ?
        result &= crystal.hasEnergy;
        if (!crystal.hasEnergy)
            Debug.LogWarning("CRYSTAL IS DEPLEATED");

        return result;
    }

    #endregion

    #region Retrieve

    #endregion

    #region Fire

    public void Fire()
    {
        if (CanFire())
        {
            if (storedEnergyCount > 0)
            {
                storedEnergyCount--;
                onRefreshEnergy.Invoke();
            }
            else
                crystal.DecrementEnergy();

            m_bulletChannel.onInstantiateBullet.Invoke(
                this,
                m_settings.bulletPrefab,
                m_bulletOrigin.position,
                m_aimDirection
            );

            if (storedEnergyCount == 0)
                onCrystalEnergyAvailable.Invoke();
        }
    }

    public bool CanFire()
    {
        bool result = true;

        // Is extracting ?
        result &= !isExtracting;
        if (isExtracting)
            Debug.LogWarning("IS EXTRACTING");

        // Has energy in crystal or stored ?
        bool hasEnergy = crystal.hasEnergy || storedEnergyCount > 0;
        result &= hasEnergy;
        if (!hasEnergy)
            Debug.LogWarning("NO ENERGY");

        return result;
    }

    private void UpdateRotation()
    {
        m_mouseWorldSpace = m_mousePositionActionReference.action.ReadValue<Vector2>();
        m_mouseWorldSpace = m_camera.ScreenToWorldPoint(m_mouseWorldSpace);
        m_mouseWorldSpace.z = 0;

        m_aimDirection = (m_mouseWorldSpace - transform.position).normalized;
        m_turret.up = m_aimDirection;
    }

    #endregion

    #endregion
}
