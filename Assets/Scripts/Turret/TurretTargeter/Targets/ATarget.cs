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

        [Header("Visual")]
        [SerializeField]
        private Color m_targeterColor;

        public Color targeterColor
        {
            get
            {
                return m_targeterColor;
            }
        }

        [Header("Transform")]
        [SerializeField]
        protected float m_targeterScaleFactor = 1f;
        public Vector3 targeterScale { get; private set; }

        protected virtual void Awake()
        {
            targeterScale = new Vector3(m_targeterScaleFactor, m_targeterScaleFactor, m_targeterScaleFactor);
        }

        public virtual Quaternion GetTargeterRotation()
        {
            return transform.rotation;
        }

        public virtual string GetInfos()
        {
            return m_type.ToString();
        }

        #endregion

    }
}
