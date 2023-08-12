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

        public override string GetInfos()
        {
            string infos = "TURRET \r\n";
            infos += string.Format("<size=50%>STATUS : {0}</size>", (TurretStateType)turret.currentState.type);
            return infos;
        }

        #endregion
    }
}
