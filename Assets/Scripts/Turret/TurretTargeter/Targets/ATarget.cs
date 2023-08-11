using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{
    public enum TargetType
    {
        None,
        Turret,
        CrystalShard
    }

    [RequireComponent(typeof(Collider2D))]
    public abstract class ATarget : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        protected Transform m_origin;

        [SerializeField]
        private TargetType m_type = TargetType.None;
        public TargetType type
        {
            get { return m_type; }
        }

        [SerializeField]
        protected float m_targeterScaleFactor = 1f;

        public Vector3 targeterScale
        {
            get { return new Vector3(m_targeterScaleFactor, m_targeterScaleFactor, m_targeterScaleFactor); }
        }

        #endregion

    }
}
