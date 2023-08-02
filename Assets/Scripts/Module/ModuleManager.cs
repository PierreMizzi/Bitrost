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
    private List<Module> m_modules = new List<Module>();

    private int m_remainingModuleCount = 0;

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

        CreateModule();
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

        if (m_retrieveNodeActionReference != null)
            m_retrieveNodeActionReference.action.performed += CallbackRetrieveNodeAction;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed += CallbackFireAction;

        if (m_extractActionReference != null)
            m_extractActionReference.action.performed += CallbackExtractAction;
    }

    private void Unsubscribe()
    {
        if (m_dropNodeActionReference != null)
            m_dropNodeActionReference.action.performed -= CallbackActivateModule;

        if (m_retrieveNodeActionReference != null)
            m_retrieveNodeActionReference.action.performed -= CallbackRetrieveNodeAction;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed -= CallbackFireAction;

        if (m_extractActionReference != null)
            m_extractActionReference.action.performed -= CallbackExtractAction;
    }

    #region Inputs

    private void CallbackActivateModule(InputAction.CallbackContext context)
    {
        Vector3 mouseScreenPosition = m_mousePositionActionReference.action.ReadValue<Vector2>();
        Vector3 raycastOrigin = m_camera.ScreenToWorldPoint(mouseScreenPosition);
        raycastOrigin.z = m_camera.transform.position.z;

        List<RaycastHit2D> results = new List<RaycastHit2D>();

        if (Physics2D.Raycast(raycastOrigin, Vector3.forward, m_crystalShardFilter, results) > 0)
        {
            if (results[0].transform.TryGetComponent<CrystalShard>(out CrystalShard crystal))
                ActivateModule(crystal);
        }
    }

    // TODO : Make mouse position related option
    private void CallbackRetrieveNodeAction(InputAction.CallbackContext context)
    {
        foreach (Module module in m_modules)
        {
            module.SetInactive();
            module.RemoveCrystal();
        }
    }

    private void CallbackFireAction(InputAction.CallbackContext context)
    {
        foreach (Module module in m_modules)
        {
            if (module.isActive)
                module.Fire();
        }
    }

    private void CallbackExtractAction(InputAction.CallbackContext context)
    {
        foreach (Module module in m_modules)
            module.ToggleExtracting();
    }

    #endregion

    #region Module

    [Obsolete]
    private bool CanCreateModule(CrystalShard crystal)
    {
        bool result = true;

        bool hasRemainingModule = m_remainingModuleCount > 0;
        result &= hasRemainingModule;
        if (!hasRemainingModule)
            Debug.LogWarning("NO AVAILABLE MODULE");

        bool isCrystalAvailable = crystal.isAvailable;
        result &= isCrystalAvailable;
        if (!isCrystalAvailable)
            Debug.LogWarning("CRYSTAL IS NOT AVAILABLE");

        return result;
    }

    [Obsolete]
    private void CreateModule(CrystalShard crystal)
    {
        if (CanCreateModule(crystal))
        {
            Module module = Instantiate(
                m_modulePrefab,
                crystal.transform.position,
                Quaternion.identity,
                m_moduleContainer
            );

            module.Initialize(this, crystal);

            m_remainingModuleCount--;
            m_modules.Add(module);
        }
    }

    #endregion

    #region Module Views


    private void CreateModule()
    {
        // Model
        Module module = Instantiate(m_modulePrefab, m_moduleContainer);
        module.Initialize(this);
        m_modules.Add(module);

        // View
        ModuleView moduleView = new ModuleView();

        // View VisualElement
        VisualElement moduleViewVisual = m_moduleViewAsset.Instantiate();
        moduleViewVisualContainer.Add(moduleViewVisual);
        moduleView.Initialize(module, moduleViewVisual);
        m_moduleViews.Add(moduleView);
    }

    private void ActivateModule(CrystalShard crystal)
    {
        Module module = GetUnactivatedModule();

        if (module != null)
        {
            module.AssignCrystal(crystal);
            module.SetActive();
        }
    }

    private Module GetUnactivatedModule()
    {
        int count = m_modules.Count;
        for (int i = 0; i < count; i++)
        {
            if (!m_modules[i].isActive)
                return m_modules[i];
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
            ModuleTarget target = FindFirst(results, TargetType.Module);

            if (target != null)
            {
                return;
            }


            ATarget target = FindFirst(results, TargetType.CrystalShard);
            if (target != null)
            {
                ManageCrystalTarget(target);
                return;
            }
        }
        else
        {
            if (m_currentTarget != null)
                UnsetTarget();
        }
    }

    private void ManageCrystalTarget(ATarget target)
    {
        m_moduleTargeter.Target(target);
        m_currentTarget = target;

        if (m_remainingModuleCount > 0)
        {
            Module module = GetUnactivatedModule();
            module.isActivable = true;
        }
    }

    private void UnsetTarget()
    {
        m_moduleTargeter.Hide();
        m_currentTarget = null;

        if (m_remainingModuleCount > 0)
        {
            Module module = GetUnactivatedModule();
            module.isActivable = false;
        }
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
