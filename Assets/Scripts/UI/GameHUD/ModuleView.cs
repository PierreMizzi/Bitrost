using UnityEngine;
using UnityEngine.UIElements;

// ModuleManager -> Module -> ModuleHUD -> Module(VisualElement)
// ModuleHUD

public class ModuleView
{
    /*
        - Switch state Active / Inactive
        - Switch mode FIRE / EXTRACT
            - Change Label
            - FIRE MODE
                - Change border on energy type
                - Hide arrow
                - Display "Fire" tip
            - EXTRACT MODE
                - Hide border on energy type
                - Display arrow
                - Hide "Fire" tip
    */

	#region Fields

	#region Behaviour


    private VisualElement m_root;

    private Module m_module;

	#endregion

	#region Status

    private const string k_activeContainer = "module-hud-active";
    private const string k_inactiveContainer = "module-hud-inactive";

    private VisualElement m_activeContainer;
    private VisualElement m_inactiveContainer;

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
        SetState(false);

        SubscribeToModel();
    }

    private void InitializeVisualElements()
    {
        // Status
        m_activeContainer = m_root.Q(k_activeContainer);
        m_inactiveContainer = m_root.Q(k_inactiveContainer);
    }

    private void SubscribeToModel()
    {
        // Status
    }

    private void UnsubscribeToModel() { }

	#endregion

	#region Status

    private void CallbackChangeState(bool state)
    {
        SetState(state);
    }

    private void SetState(bool state)
    {
        m_activeContainer.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
        m_inactiveContainer.style.display = state ? DisplayStyle.None : DisplayStyle.Flex;
    }

	#endregion

	#endregion
}
