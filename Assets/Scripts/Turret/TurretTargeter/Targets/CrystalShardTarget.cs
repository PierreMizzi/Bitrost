using Bitfrost.Gameplay.Energy;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{
    public class CrystalShardTarget : ATarget
    {
        #region Fields

        public CrystalShard crystal { get; private set; }

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            crystal = m_origin.GetComponent<CrystalShard>();
            m_targeterScaleFactor *= m_origin.localScale.x;
        }

        public override string GetInfos()
        {
            string infos = "Asteroid \r\n";
            infos += "Energy : " + crystal.remainingEnergyCount;
            return infos;
        }

        #endregion
    }
}
