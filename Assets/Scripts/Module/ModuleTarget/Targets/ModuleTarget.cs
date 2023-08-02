public class ModuleTarget : ATarget
{
	#region Fields

    public Module turret { get; private set; }

	#endregion

	#region Methods

    private void Awake()
    {
        turret = m_origin.GetComponent<Module>();
    }

	#endregion
}
