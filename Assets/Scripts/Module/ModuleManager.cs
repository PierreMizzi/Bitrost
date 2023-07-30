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

    #region Bullets

    [Header("Bullets")]
    [SerializeField]
    private Transform m_bulletContainer = null;
    public Transform bulletContainer
    {
        get { return m_bulletContainer; }
    }

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

        Initialize();
        CreateModule();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

	#endregion

    private void Subscribe()
    {
        if (m_dropNodeActionReference != null)
            m_dropNodeActionReference.action.performed += CallbackDropNodeAction;

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
            m_dropNodeActionReference.action.performed -= CallbackDropNodeAction;

        if (m_retrieveNodeActionReference != null)
            m_retrieveNodeActionReference.action.performed -= CallbackRetrieveNodeAction;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed -= CallbackFireAction;

        if (m_extractActionReference != null)
            m_extractActionReference.action.performed -= CallbackExtractAction;
    }

    #region Inputs

    private void CallbackDropNodeAction(InputAction.CallbackContext context)
    {
        Vector3 mouseScreenPosition = m_mousePositionActionReference.action.ReadValue<Vector2>();
        Vector3 raycastOrigin = m_camera.ScreenToWorldPoint(mouseScreenPosition);
        raycastOrigin.z = m_camera.transform.position.z;

        List<RaycastHit2D> results = new List<RaycastHit2D>();

        if (Physics2D.Raycast(raycastOrigin, Vector3.forward, m_crystalShardFilter, results) > 0)
        {
            if (results[0].transform.TryGetComponent<CrystalShard>(out CrystalShard crystal))
                CreateModule(crystal);
        }
    }

    private void CallbackRetrieveNodeAction(InputAction.CallbackContext context)
    {
        foreach (Module module in m_modules)
        {
            module.Retrieve();
            Destroy(module.gameObject);
        }
        m_modules.Clear();
        m_remainingModuleCount = m_settings.startingModuleCount;
    }

    private void CallbackFireAction(InputAction.CallbackContext context)
    {
        foreach (Module module in m_modules)
            module.Fire();
    }

    private void CallbackExtractAction(InputAction.CallbackContext context)
    {
        foreach (Module module in m_modules)
            module.ToggleExtracting();
    }

    #endregion

    #region Module

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

    #region Rework

    [SerializeField]
    private UIDocument m_document = null;

    private const string k_moduleViewVisualContainer = "module-container";

    public VisualElement moduleViewVisualContainer { get; private set; }
    
    [SerializeField]
    private VisualTreeAsset m_moduleViewAsset;

    public List<ModuleView> m_moduleViews = new List<ModuleView>();

    // TODO : Initialize
    private void Initialize()
    {
        moduleViewVisualContainer = m_document.rootVisualElement.Q(k_moduleViewVisualContainer);
    }

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

    private void ActivateModule()
    {
        Module module = GetUnactivatedModule();

        if (module)
        {
            Debug.LogWarning("NO UNACTIVATED MODULE");
            return;
        }



    }

    private Module GetUnactivatedModule()
    {
        int count = m_modules.Count;
        for (int i = 0; i < count; i++)
        {
            if (!m_modules[i].isActivated)
                return m_modules[i];
        }
        return null;
    }

    #endregion



    #endregion
}
