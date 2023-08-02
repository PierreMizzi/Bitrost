using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public delegate void ModuleDelegate(Module module);

/*
    Targeted when Fire mode
    -> Retrieve on E
    -> Extract Mode

    Targeted when Extract
    -> Retrieve on E
    -> Fire Mode
*/



// TODO : back to Fire Mode when retrieving while extracting
// TODO : Back to Fire Mode when can't extract any longer
// TODO : Small message when can't fire because extract mode
public partial class Module : MonoBehaviour, IBulletLauncher
{
    #region Fields

    #region Behaviour

    [Header("Behaviour")]
    [SerializeField]
    private ModuleSettings m_settings;

    public ModuleSettings settings
    {
        get { return m_settings; }
    }

    [SerializeField]
    private BulletChannel m_bulletChannel;

    private ModuleManager m_manager;

    public CrystalShard crystal { get; private set; }

    #endregion

    #region Offensive

    [Header("Offensive")]
    [SerializeField]
    private InputActionReference m_mousePositionInput = null;

    [SerializeField]
    private Transform m_canonTransform;

    [SerializeField]
    private Transform m_bulletOriginTransform;

    private Vector3 m_mouseWorldSpace;

    public new Camera camera { get; private set; }

    public Vector3 aimDirection { get; private set; }

    public Transform canonTransform
    {
        get { return m_canonTransform; }
    }

    #endregion

    #endregion

    #region Extracting

    [Obsolete]
    public bool isExtracting { get; private set; }

    public float productionProgress { get; private set; }

    public int m_storedEnergy;

    public int storedEnergy
    {
        get { return m_storedEnergy; }
        set { m_storedEnergy = Mathf.Clamp(value, 0, m_settings.maxStoredEnergy); }
    }

    public bool hasEnergy
    {
        get { return crystal.hasEnergy || m_storedEnergy > 0; }
    }

    #endregion

    #region Reworked UI

    [SerializeField]
    private GameObject m_spritesContainer = null;

    private bool m_isDroppable;

    public bool isDroppable
    {
        get { return m_isDroppable; }
        set
        {
            m_isDroppable = value;
            onDroppable?.Invoke();
        }
    }

    [SerializeField]
    private bool m_isTargeted = false;
    public bool isTargeted
    {
        get { return m_isTargeted; }
        set
        {
            m_isTargeted = value;
            onSetTargeted?.Invoke();
        }
    }

    // Activation
    public bool isActive
    {
        get { return (TurretStateType)currentState.type != TurretStateType.Inactive; }
    }

    public Action onRefreshEnergy = null;
    public Action onDroppable = null;
    public Action onSetTargeted = null;
    public Action onSetActive = null;
    public Action onAssignCrystal = null;
    public Action onRemoveCrystal = null;
    public Action onExtraction = null;
    public FloatDelegate onUpdateExtractionUI = null;

    #endregion

    #region Methods

    public void Initialize(ModuleManager manager)
    {
        m_manager = manager;
        camera = Camera.main;

        storedEnergy = 0;

        ChangeState(TurretStateType.Inactive);
    }

    #region MonoBehaviour

    protected void Awake()
    {
        camera = Camera.main;

        InitiliazeStates();

        onDroppable = () => { };
        onSetActive = () => { };
        onAssignCrystal = () => { };
        onRemoveCrystal = () => { };

        onRefreshEnergy = () => { };
        onUpdateExtractionUI = (float normalized) => { };
    }

    public void Update()
    {
        UpdateState();
    }

    #endregion

    #region Activation

    public void SetActive()
    {
        m_spritesContainer.SetActive(true);
        onSetActive.Invoke();

        onRefreshEnergy.Invoke();
        onExtraction.Invoke();
    }

    public void SetInactive()
    {
        m_spritesContainer.SetActive(false);

        onSetActive.Invoke();
    }

    public void AssignCrystal(CrystalShard crystal)
    {
        this.crystal = crystal;
        this.crystal.SetUnavailable();

        transform.position = this.crystal.transform.position;

        onAssignCrystal.Invoke();
        this.crystal.onRefreshEnergy.Invoke();
    }

    public void RemoveCrystal()
    {
        crystal.SetAvailable();

        if (!crystal.hasEnergy)
            crystal.Release();

        onRemoveCrystal.Invoke();
        crystal = null;
    }

    #endregion

    #region Targeted



    #endregion

    #region Fire

    public void ComputeAimDirection()
    {
        m_mouseWorldSpace = m_mousePositionInput.action.ReadValue<Vector2>();
        m_mouseWorldSpace = camera.ScreenToWorldPoint(m_mouseWorldSpace);
        m_mouseWorldSpace.z = 0;

        aimDirection = (m_mouseWorldSpace - transform.position).normalized;
    }

    public void Fire()
    {
        if (CanFire())
        {
            if (storedEnergy > 0)
                storedEnergy--;
            else
                crystal.DecrementEnergy();

            onRefreshEnergy();

            m_bulletChannel.onInstantiateBullet.Invoke(
                this,
                m_settings.bulletPrefab,
                m_bulletOriginTransform.position,
                aimDirection
            );

            if (!hasEnergy)
                ChangeState(TurretStateType.Disabled);
        }
    }

    public bool CanFire()
    {
        bool result = true;

        bool inOffensiveState = (TurretStateType)currentState.type == TurretStateType.Offensive;
        result &= inOffensiveState;

        if (!inOffensiveState)
            Debug.LogWarning("IS NOT IN OFFENSIVE STATE");

        return result;
    }

    #endregion

    #endregion
}
