using System;
using UnityEngine;
using UnityEngine.UIElements;

// ModuleManager -> Module -> ModuleHUD -> Module(VisualElement)
// ModuleHUD

public class ModuleView
{
    /*
        - Switch state Active / Inactive                    DONE
        - Crystal Energy remaining/Capacity                 DONE
        - Stored Energy remaining/Capacity                  DONE
        - Switch mode FIRE / EXTRACT
            - Change Label                                  DONE
            - FIRE MODE
                - Change border on energy type
                - Hide arrow                                DONE
                - Display "Fire" tip
            - EXTRACT MODE
                - Hide border on energy type
                - Display arrow                             DONE
                - Hide "Fire" tip
    */

    // TODO : Progress Bar extraction

	#region Fields

	#region Behaviour


    private VisualElement m_root;

    private Module m_module;

	#endregion

    #region Activable

    private const string k_activableLabel = "module-hud-inactive__label";
    private Label m_activableLabel;
    private const string k_isNotActivableText = "Search asteroid";
    private const string k_isActivableText = "<b>Press E</b> to drop module";

    #endregion

	#region Activation

    private const string k_activeContainer = "module-hud-active";
    private const string k_inactiveContainer = "module-hud-inactive";

    private VisualElement m_activeContainer;
    private VisualElement m_inactiveContainer;

	#endregion


    #region Mode

    private const string k_modeLabel = "module-hud__mode-label";
    private const string k_extractionArrow = "resources-arrow";
    private const string k_crystalEnergyContainer = "resources-crystal-container";
    private const string k_storedEnergyContainer = "resources-stored-container";
    private const string k_crystalEnergyLabel = "resources-crystal-label";
    private const string k_storedEnergyLabel = "resources-stored-label";

    private Label m_modelLabel;
    private VisualElement m_extractionArrow;
    private VisualElement m_crystalEnergyContainer;
    private VisualElement m_storedEnergyContainer;
    private Label m_crystalEnergyLabel;
    private Label m_storedEnergyLabel;

    private const string k_availableResourceClass = "available-resources";

    #endregion

    #region Tips

    private const string k_retrieveTip = "tip-retrieve";
    private const string k_fireTip = "tip-fire";
    private const string k_fireModeTip = "tip-fire-mode";
    private const string k_extractModeTip = "tip-extract-mode";

    private VisualElement m_retrieveTip;
    private VisualElement m_fireTip;
    private VisualElement m_fireModeTip;
    private VisualElement m_extractModeTip;

    #endregion

	#endregion


	#region Methods

	#region Behaviour

    public void Initialize(Module module, VisualElement root)
    {
        m_module = module;

        // Instantiate Visual Asset
        m_root = root;

        InitializeVisualElements();
        SetActive(false);

        SubscribeToModel();
    }

    private void InitializeVisualElements()
    {
        // Activable
        m_activableLabel = m_root.Q<Label>(k_activableLabel);

        // Targeted

        // Status
        m_activeContainer = m_root.Q(k_activeContainer);
        m_inactiveContainer = m_root.Q(k_inactiveContainer);

        m_modelLabel = m_root.Q<Label>(k_modeLabel);
        m_extractionArrow = m_root.Q(k_extractionArrow);
        m_crystalEnergyContainer = m_root.Q(k_crystalEnergyContainer);
        m_storedEnergyContainer = m_root.Q(k_storedEnergyContainer);

        // Energy
        m_crystalEnergyLabel = m_root.Q<Label>(k_crystalEnergyLabel);
        m_storedEnergyLabel = m_root.Q<Label>(k_storedEnergyLabel);

        // Tips
        m_retrieveTip = m_root.Q(k_retrieveTip);
        m_fireTip = m_root.Q(k_fireTip);
        m_fireModeTip = m_root.Q(k_fireModeTip);
        m_extractModeTip = m_root.Q(k_extractModeTip);

        m_retrieveTip.style.display = DisplayStyle.None;
        m_fireTip.style.display = DisplayStyle.None;
        m_fireModeTip.style.display = DisplayStyle.None;
        m_extractModeTip.style.display = DisplayStyle.None;
    }

    private void SubscribeToModel()
    {
        // Activable
        m_module.onDroppable += CallbackIsActivable;

        // Targeted
        m_module.onSetTargeted += CallbackIsTargeted;

        // Status
        m_module.onSetActive += CallbackOnActivation;

        // Energy
        m_module.onAssignCrystal += CallbackAssignCrystal;
        m_module.onRemoveCrystal += CallbackRemoveCrystal;

        m_module.onRefreshEnergy += CallbackRefreshEnergy;

        // Extraction
        m_module.onExtraction += CallbackExtraction;

    }

    private void CallbackAssignCrystal()
    {
        m_module.crystal.onRefreshEnergy += CallbackRefreshEnergy;
    }

    private void CallbackRemoveCrystal()
    {
        m_module.crystal.onRefreshEnergy -= CallbackRefreshEnergy;
    }

    private void UnsubscribeToModel() { }

	#endregion

    #region Activable

    private void CallbackIsActivable()
    {
        m_activableLabel.text = (m_module.isDroppable) ? k_isActivableText : k_isNotActivableText;
    }

    #endregion

    #region Targeted

    private void CallbackIsTargeted()
    {
        m_retrieveTip.style.display = m_module.isTargeted ? DisplayStyle.Flex : DisplayStyle.None;

        m_extractModeTip.style.display =
            m_module.isExtracting && m_module.isTargeted ? DisplayStyle.Flex : DisplayStyle.None;

        m_fireModeTip.style.display =
            !m_module.isExtracting && m_module.isTargeted ? DisplayStyle.Flex : DisplayStyle.None;
    }

    #endregion

	#region Activation

    private void CallbackOnActivation()
    {
        SetActive(m_module.isActive);
    }

    private void SetActive(bool state)
    {
        m_activeContainer.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
        m_inactiveContainer.style.display = state ? DisplayStyle.None : DisplayStyle.Flex;
    }

	#endregion

    #region Energy

    private void CallbackRefreshEnergy()
    {
        // Manages the priority resource
        if (m_module.storedEnergy > 0)
        {
            m_crystalEnergyContainer.RemoveFromClassList(k_availableResourceClass);
            m_storedEnergyContainer.AddToClassList(k_availableResourceClass);
        }
        else
        {
            m_crystalEnergyContainer.AddToClassList(k_availableResourceClass);
            m_storedEnergyContainer.RemoveFromClassList(k_availableResourceClass);
        }

        m_crystalEnergyLabel.text = m_module.crystal.remainingEnergyCount.ToString();

        m_storedEnergyLabel.text = string.Format(
            "{0} / {1}",
            m_module.storedEnergy,
            m_module.settings.maxStoredEnergy
        );
    }

    #endregion

    #region Extraction

    private void CallbackExtraction()
    {
        if (m_module.isExtracting)
            SetExtractionMode();
        else
            SetFireMode();
    }

    private void SetFireMode()
    {
        m_modelLabel.text = "OFFENSIVE MODE";
        m_extractionArrow.style.visibility = Visibility.Hidden;
        m_fireTip.style.display = DisplayStyle.Flex;
    }

    private void SetExtractionMode()
    {
        m_modelLabel.text = "REFFINE MODE";
        m_extractionArrow.style.visibility = Visibility.Visible;
        m_fireTip.style.display = DisplayStyle.None;
    }

    #endregion

	#endregion
}
