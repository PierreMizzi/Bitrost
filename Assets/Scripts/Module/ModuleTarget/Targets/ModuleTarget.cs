public class ModuleTarget : ATarget
{
	#region Fields

    public Module module { get; private set; }

	#endregion

	#region Methods

    private void Awake()
    {
        module = m_origin.GetComponent<Module>();
    }

	#endregion
}
