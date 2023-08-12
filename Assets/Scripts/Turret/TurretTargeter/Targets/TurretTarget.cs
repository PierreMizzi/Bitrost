namespace Bitfrost.Gameplay.Turrets
{
    public class TurretTarget : ATarget
    {
        #region Fields

        public Turret turret { get; private set; }

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            turret = m_origin.GetComponent<Turret>();
        }

        #endregion
    }
}
