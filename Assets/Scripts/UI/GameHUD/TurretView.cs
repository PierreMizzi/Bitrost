using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretView
{

    #region Fields

    #region Behaviour

    private VisualElement m_root;
    private Turret m_turret;

    #endregion

    #region Droppable

    private const string k_droppableLabel = "module-hud-inactive__label";
    private Label m_droppableLabel;
    private const string k_isNotDroppableText = "Search asteroid";
    private const string k_isDroppableText = "<b>Right click</b> to drop turret";

    #endregion

    #region Activation

    private const string k_activeContainer = "module-hud-active";
    private const string k_inactiveContainer = "module-hud-inactive";

    private VisualElement m_activeContainer;
    private VisualElement m_inactiveContainer;

    #endregion

    #region Mode

    private const string k_modeLabel = "module-hud__mode-label";
    private const string k_crystalEnergyContainer = "resources-crystal-container";
    private const string k_storedEnergyContainer = "resources-stored-container";
    private const string k_crystalEnergyLabel = "resources-crystal-label";
    private const string k_storedEnergyLabel = "resources-stored-label";
    private const string k_productionProgressContainer = "production-progress-container";
    private const string k_productionProgressFill = "production-progress-fill";

    private Label m_modelLabel;
    private VisualElement m_crystalEnergyContainer;
    private VisualElement m_storedEnergyContainer;
    private Label m_crystalEnergyLabel;
    private Label m_storedEnergyLabel;
    private VisualElement m_productionProgressContainer;
    private VisualElement m_productionProgressFill;

    private const string k_availableResourceClass = "available-resources";

    #endregion

    #region Tips

    private const string k_retrieveTip = "tip-retrieve";
    private const string k_fireTip = "tip-fire";
    private const string k_offensiveModeTip = "tip-fire-mode";
    private const string k_productionModeTip = "tip-extract-mode";

    private VisualElement m_retrieveTip;
    private VisualElement m_fireTip;
    private VisualElement m_offensiveModeTip;
    private VisualElement m_productionModeTip;

    #endregion

    #endregion

    #region Methods

    #region Behaviour

    public void Initialize(Turret turret, VisualElement root)
    {
        m_turret = turret;

        // Instantiate Visual Asset
        m_root = root;

        InitializeVisualElements();

        SubscribeToModel();

        CallbackChangeState(TurretStateType.Inactive);
    }

    private void InitializeVisualElements()
    {
        // Activable
        m_droppableLabel = m_root.Q<Label>(k_droppableLabel);

        // Status
        m_activeContainer = m_root.Q(k_activeContainer);
        m_inactiveContainer = m_root.Q(k_inactiveContainer);

        m_modelLabel = m_root.Q<Label>(k_modeLabel);
        m_productionProgressContainer = m_root.Q(k_productionProgressContainer);
        m_crystalEnergyContainer = m_root.Q(k_crystalEnergyContainer);
        m_storedEnergyContainer = m_root.Q(k_storedEnergyContainer);

        // Energy
        m_crystalEnergyLabel = m_root.Q<Label>(k_crystalEnergyLabel);
        m_storedEnergyLabel = m_root.Q<Label>(k_storedEnergyLabel);

        m_productionProgressFill = m_root.Q(k_productionProgressFill);

        // Tips
        m_retrieveTip = m_root.Q(k_retrieveTip);
        m_fireTip = m_root.Q(k_fireTip);
        m_offensiveModeTip = m_root.Q(k_offensiveModeTip);
        m_productionModeTip = m_root.Q(k_productionModeTip);

        m_retrieveTip.style.display = DisplayStyle.None;
        m_fireTip.style.display = DisplayStyle.None;
        m_offensiveModeTip.style.display = DisplayStyle.None;
        m_productionModeTip.style.display = DisplayStyle.None;
    }

    private void SubscribeToModel()
    {
        // Activable
        m_turret.onIsDroppable += CallbackIsDroppable;

        // Targeted
        m_turret.onIsTargeted += CallbackIsTargeted;

        // Status
        m_turret.onChangeState += CallbackChangeState;

        // Energy
        m_turret.onAssignCrystal += CallbackAssignCrystal;
        m_turret.onRemoveCrystal += CallbackRemoveCrystal;

        m_turret.onRefreshEnergy += CallbackRefreshEnergy;
        m_turret.onProductionProgress += CallbackProductionProgress;
    }



    private void CallbackAssignCrystal()
    {
        m_turret.crystal.onRefreshEnergy += CallbackRefreshEnergy;
    }

    private void CallbackRemoveCrystal()
    {
        m_turret.crystal.onRefreshEnergy -= CallbackRefreshEnergy;
    }

    private void UnsubscribeToModel() { }

    #endregion

    #region Activable

    private void CallbackIsDroppable()
    {
        m_droppableLabel.text = m_turret.isDroppable ? k_isDroppableText : k_isNotDroppableText;
    }

    #endregion

    #region Targeted

    private void CallbackIsTargeted()
    {
        m_retrieveTip.style.display = m_turret.isTargeted ? DisplayStyle.Flex : DisplayStyle.None;

        TurretStateType state = (TurretStateType)m_turret.currentState.type;

        if (m_turret.isTargeted)
        {
            m_productionModeTip.style.display = state == TurretStateType.Offensive ? DisplayStyle.Flex : DisplayStyle.None;
            m_offensiveModeTip.style.display = state == TurretStateType.Production ? DisplayStyle.Flex : DisplayStyle.None;
        }
        else
        {
            m_productionModeTip.style.display = DisplayStyle.None;
            m_offensiveModeTip.style.display = DisplayStyle.None;
        }
    }

    #endregion

    #region State

    private void CallbackChangeState(TurretStateType type)
    {
        switch (type)
        {
            case TurretStateType.Inactive:
                SetStateInactive();
                break;
            case TurretStateType.Offensive:
                SetStateOffensive();
                break;
            case TurretStateType.Production:
                SetStateProduction();
                break;
            case TurretStateType.Disabled:
                SetStateDisabled();
                break;
        }
    }

    private void SetStateInactive()
    {
        m_activeContainer.style.display = DisplayStyle.None;
        m_inactiveContainer.style.display = DisplayStyle.Flex;
    }
    private void SetStateOffensive()
    {
        m_activeContainer.style.display = DisplayStyle.Flex;
        m_inactiveContainer.style.display = DisplayStyle.None;

        m_modelLabel.text = "ATTACK";
        m_productionProgressContainer.style.visibility = Visibility.Hidden;
        m_fireTip.style.display = DisplayStyle.Flex;
    }

    private void SetStateProduction()
    {
        m_activeContainer.style.display = DisplayStyle.Flex;
        m_inactiveContainer.style.display = DisplayStyle.None;

        m_modelLabel.text = "PRODUCE";
        m_productionProgressContainer.style.visibility = Visibility.Visible;
        m_fireTip.style.display = DisplayStyle.None;
    }

    private void SetStateDisabled()
    {
        m_modelLabel.text = "NO ENERGY";

        m_crystalEnergyContainer.RemoveFromClassList(k_availableResourceClass);
        m_storedEnergyContainer.RemoveFromClassList(k_availableResourceClass);
    }

    #endregion

    #region Production

    private void CallbackRefreshEnergy()
    {
        // Manages the priority resource
        if (m_turret.storedEnergy > 0)
        {
            m_crystalEnergyContainer.RemoveFromClassList(k_availableResourceClass);
            m_storedEnergyContainer.AddToClassList(k_availableResourceClass);
        }
        else
        {
            m_crystalEnergyContainer.AddToClassList(k_availableResourceClass);
            m_storedEnergyContainer.RemoveFromClassList(k_availableResourceClass);
        }

        m_crystalEnergyLabel.text = m_turret.crystal.remainingEnergyCount.ToString();

        m_storedEnergyLabel.text = string.Format(
            "{0} / {1}",
            m_turret.storedEnergy,
            m_turret.settings.maxStoredEnergy
        );
    }

    private void CallbackProductionProgress(float value)
    {
        m_productionProgressFill.style.width = Length.Percent(value * 100f);
    }

    #endregion

    #region Destroy

    #endregion

    #endregion

}
