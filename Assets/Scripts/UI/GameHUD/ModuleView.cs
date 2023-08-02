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
    private const string k_fireTip = "tips-fire";

    private Label m_modelLabel;
    private VisualElement m_extractionArrow;
    private VisualElement m_crystalEnergyContainer;
    private VisualElement m_storedEnergyContainer;
    private Label m_crystalEnergyLabel;
    private Label m_storedEnergyLabel;
    private VisualElement m_fireTip;

    private const string k_availableResourceClass = "available-resources";

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

        // Mode
        m_fireTip = m_root.Q(k_fireTip);
    }

    private void SubscribeToModel()
    {
        // Status
        m_module.onActivation += CallbackOnActivation;

        // Energy
        m_module.onAssignCrystal += CallbackAssignCrystal;
        m_module.onRemoveCrystal += CallbackRemoveCrystal;

        m_module.onRefreshEnergy += CallbackRefreshStoredEnergy;

        // Extraction
        m_module.onExtraction += CallbackExtraction;

        // Available Energy
        m_module.onStoredEnergyAvailable += CallbackStoredEnergyAvailable;
        m_module.onCrystalEnergyAvailable += CallbackCrystalEnergyAvailable;
    }

    private void CallbackAssignCrystal()
    {
        m_module.crystal.onRefreshEnergy += CallbackRefreshCrystalEnergy;
    }

    private void CallbackRemoveCrystal()
    {
        m_module.crystal.onRefreshEnergy -= CallbackRefreshCrystalEnergy;
    }

    private void UnsubscribeToModel() { }

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

    private void CallbackRefreshCrystalEnergy()
    {
        m_crystalEnergyLabel.text = m_module.crystal.remainingEnergyCount.ToString();
    }

    private void CallbackRefreshStoredEnergy()
    {
        m_storedEnergyLabel.text = string.Format(
            "{0} / {1}",
            m_module.storedEnergyCount,
            m_module.storedEnergyCapacity
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
        m_modelLabel.text = "Fire";
        m_extractionArrow.style.visibility = Visibility.Hidden;
        m_fireTip.style.display = DisplayStyle.Flex;
    }

    private void SetExtractionMode()
    {
        m_modelLabel.text = "Extract";
        m_extractionArrow.style.visibility = Visibility.Visible;
        m_fireTip.style.display = DisplayStyle.None;
    }

    #endregion

    #region Available Energy

    private void CallbackCrystalEnergyAvailable()
    {
        m_crystalEnergyContainer.AddToClassList(k_availableResourceClass);
        m_storedEnergyContainer.RemoveFromClassList(k_availableResourceClass);
    }

    private void CallbackStoredEnergyAvailable()
    {
        m_storedEnergyContainer.AddToClassList(k_availableResourceClass);
        m_crystalEnergyContainer.RemoveFromClassList(k_availableResourceClass);
    }

    #endregion

	#endregion
}
