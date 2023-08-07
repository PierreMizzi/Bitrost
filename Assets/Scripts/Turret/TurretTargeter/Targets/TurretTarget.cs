public class TurretTarget : ATarget
{
    #region Fields

    public Turret turret { get; private set; }

    #endregion

    #region Methods

    private void Awake()
    {
        turret = m_origin.GetComponent<Turret>();
    }

    #endregion
}
