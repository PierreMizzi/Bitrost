using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/*

    - Controller : Inputs from mouse, checks closer Module
    - Model : Manages available controllers
    - View : Creates ModuleHUD VisualElements, links them to Module

*/

public class ModuleManager : MonoBehaviour
{
    #region Fields

    #region Inputs

    private Camera m_camera;

    [SerializeField]
    private ModuleSettings m_settings = null;

    [Header("Inputs")]
    [SerializeField]
    private ContactFilter2D m_crystalShardFilter;

    [SerializeField]
    private InputActionReference m_mousePositionActionReference = null;

    [SerializeField]
    private InputActionReference m_dropNodeActionReference = null;

    [SerializeField]
    private InputActionReference m_retrieveNodeActionReference = null;

    [SerializeField]
    private InputActionReference m_fireActionReference = null;

    [SerializeField]
    private InputActionReference m_extractActionReference = null;

    #endregion

    #region Modules

    [Header("Modules")]
    [SerializeField]
    private Module m_modulePrefab = null;

    [SerializeField]
    private Transform m_moduleContainer = null;

    [SerializeField]
    private List<Module> m_turrets = new List<Module>();

    private int m_remainingModuleCount = 0;

    private bool hasAvailableTurret
    {
        get { return m_remainingModuleCount > 0; }
    }

    #endregion

    #region ModuleViews

    [SerializeField]
    private UIDocument m_document = null;

    private const string k_moduleViewVisualContainer = "module-container";

    public VisualElement moduleViewVisualContainer { get; private set; }

    [SerializeField]
    private VisualTreeAsset m_moduleViewAsset;

    public List<ModuleView> m_moduleViews = new List<ModuleView>();

    #endregion

    #region Module Target



    #endregion

    #endregion

    #region Methods

    #region MonoBehaviour

    private void OnEnable()
    {
        m_camera = Camera.main;
    }

    private void Start()
    {
        m_remainingModuleCount = m_settings.startingModuleCount;
        Subscribe();

        moduleViewVisualContainer = m_document.rootVisualElement.Q(k_moduleViewVisualContainer);

        CreateTurret();
    }

    private void Update()
    {
        ManageTarget();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    #endregion

    private void Subscribe()
    {
        if (m_dropNodeActionReference != null)
            m_dropNodeActionReference.action.performed += CallbackActivateModule;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed += CallbackFireAction;

        if (m_extractActionReference != null)
            m_extractActionReference.action.performed += CallbackSwitchModeAction;
    }

    private void Unsubscribe()
    {
        if (m_dropNodeActionReference != null)
            m_dropNodeActionReference.action.performed -= CallbackActivateModule;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed -= CallbackFireAction;

        if (m_extractActionReference != null)
            m_extractActionReference.action.performed -= CallbackSwitchModeAction;
    }

    #region Inputs

    private void CallbackActivateModule(InputAction.CallbackContext context)
    {
        if (m_currentTarget == null)
            return;

        switch (m_currentTarget.type)
        {
            case TargetType.CrystalShard:
                ActivateAvailableModule();
                break;
            case TargetType.Turret:
                InactivateTurret();
                break;
            default:
                break;
        }
    }

    private void ActivateAvailableModule()
    {
        CrystalShard crystal = ((CrystalShardTarget)m_currentTarget).crystal;

        if (crystal.isAvailable)
        {
            if (hasAvailableTurret)
                DeployTurret(crystal);
            else
            { /* TODO : Redeploy Turret*/
            }
        }
    }

    private void InactivateTurret()
    {
        Module turret = ((ModuleTarget)m_currentTarget).turret;

        turret.RemoveCrystal();
        turret.ChangeState(TurretStateType.Inactive);
    }

    private void CallbackFireAction(InputAction.CallbackContext context)
    {
        foreach (Module turret in m_turrets)
            turret.Fire();
    }

    private void CallbackSwitchModeAction(InputAction.CallbackContext context)
    {
        if (m_currentTarget == null)
            return;

        switch (m_currentTarget.type)
        {
            case TargetType.Turret:
                SwitchTurretMode();
                break;
            default:
                break;
        }
    }

    private void SwitchTurretMode()
    {
        Module turret = ((ModuleTarget)m_currentTarget).turret;

        turret.SwitchMode();
    }

    #endregion


    #region Turret

    private void CreateTurret()
    {
        // Instantiate Model
        Module turret = Instantiate(m_modulePrefab, m_moduleContainer);
        turret.Initialize(this);
        m_turrets.Add(turret);

        // Instiate View, link it to model
        ModuleView turretView = new ModuleView();
        VisualElement turretViewVisual = m_moduleViewAsset.Instantiate();
        moduleViewVisualContainer.Add(turretViewVisual);
        turretView.Initialize(turret, turretViewVisual);
        m_moduleViews.Add(turretView);
    }

    private void DeployTurret(CrystalShard crystal)
    {
        Module turret = GetInactiveTurret();

        if (turret != null)
        {
            turret.AssignCrystal(crystal);
            turret.ChangeState(TurretStateType.Offensive);
        }
    }

    private Module GetInactiveTurret()
    {
        int count = m_turrets.Count;
        for (int i = 0; i < count; i++)
        {
            if ((TurretStateType)m_turrets[i].currentState.type == TurretStateType.Inactive)
                return m_turrets[i];
        }
        return null;
    }

    #endregion

    #region ModuleTarget

    [SerializeField]
    private ModuleTargeter m_moduleTargeter;

    [SerializeField]
    private ContactFilter2D m_targetFilter;

    private ATarget m_currentTarget;

    List<RaycastHit2D> results = new List<RaycastHit2D>();

    public void ManageTarget()
    {
        Vector3 mouseScreenPosition = m_mousePositionActionReference.action.ReadValue<Vector2>();
        Vector3 raycastOrigin = ScreenPositionToRaycastOrigin(mouseScreenPosition);

        if (Physics2D.Raycast(raycastOrigin, Vector3.forward, m_targetFilter, results) > 0)
        {
            ATarget target = FindFirst(results, TargetType.Turret);
            if (target != null)
            {
                ManageModuleTarget(target);
                return;
            }

            target = FindFirst(results, TargetType.CrystalShard);
            if (target != null)
            {
                ManageCrystalTarget(target);
                return;
            }
        }
        else
        {
            if (m_currentTarget != null)
            {
                switch (m_currentTarget.type)
                {
                    case TargetType.CrystalShard:
                        UnsetCrystalTarget();
                        break;
                    case TargetType.Turret:
                        UnsetModuleTarget();
                        break;
                }
            }
        }
    }

    private void ManageCrystalTarget(ATarget target)
    {
        m_moduleTargeter.Target(target);
        m_currentTarget = target;

        if (m_remainingModuleCount > 0)
        {
            Module module = GetInactiveTurret();
            if (module != null)
                module.isDroppable = true;
        }
    }

    private void ManageModuleTarget(ATarget target)
    {
        m_moduleTargeter.Target(target);
        m_currentTarget = target;

        ((ModuleTarget)m_currentTarget).turret.isTargeted = true;
    }

    private void UnsetCrystalTarget()
    {
        m_moduleTargeter.Hide();
        m_currentTarget = null;

        if (m_remainingModuleCount > 0)
        {
            Module module = GetInactiveTurret();
            if (module != null)
                module.isDroppable = false;
        }
    }

    private void UnsetModuleTarget()
    {
        ((ModuleTarget)m_currentTarget).turret.isTargeted = false;

        m_moduleTargeter.Hide();
        m_currentTarget = null;
    }

    public ATarget FindFirst(List<RaycastHit2D> results, TargetType type)
    {
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent<ATarget>(out ATarget target))
            {
                if (target.type == type)
                    return target;
            }
        }
        return null;
    }

    public Vector3 ScreenPositionToRaycastOrigin(Vector2 screenPosition)
    {
        Vector3 raycastOrigin = m_camera.ScreenToWorldPoint(screenPosition);
        raycastOrigin.z = m_camera.transform.position.z;
        return raycastOrigin;
    }

    #endregion

    #endregion
}
