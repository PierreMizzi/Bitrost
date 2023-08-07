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

        private void Awake()
        {
            crystal = m_origin.GetComponent<CrystalShard>();
            m_targeterScaleFactor *= m_origin.localScale.x;
        }

        #endregion
    }
}
