using Bitfrost.Gameplay.Energy;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{
    public class CrystalShardTarget : ATarget
    {
        #region Fields

        public CrystalShard crystal { get; private set; }

        private Quaternion m_offsetRotation = Quaternion.Euler(0, 0, -35f);

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            crystal = m_origin.GetComponent<CrystalShard>();
            m_targeterScaleFactor *= m_origin.localScale.x;
        }

        public override Quaternion GetTargeterRotation()
        {
            return crystal.spriteRenderer.transform.rotation * m_offsetRotation;
        }

        public override string GetInfos()
        {
            string infos = "ASTEROID \r\n";
            infos += string.Format("<size=50%>ENERGY : {0} </size>", crystal.remainingEnergyCount);
            return infos;
        }

        #endregion
    }
}
