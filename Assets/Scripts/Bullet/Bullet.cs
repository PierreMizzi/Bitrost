using PierreMizzi.Useful;
using UnityEngine;

namespace Bitfrost.Gameplay.Bullets
{

    public class Bullet : MonoBehaviour, IPausable
    {
        #region Fields

        [Header("Bullet")]
        protected BulletManager m_manager = null;

        [SerializeField] protected BulletChannel m_bulletChannel = null;

        [SerializeField] protected float m_speed;
        [SerializeField] protected float m_damage;

        protected IBulletLauncher m_launcher;

        [Header("Impact")]

        [SerializeField]
        private BulletImpact m_impactPrefab;

        #region Collision

        protected bool m_hasHit = false;

        [SerializeField]
        protected ContactFilter2D m_collisionFilter;

        #endregion

        public bool isPaused { get; set; }

        #endregion

        #region Methods

        #region MonoBehaviour

        protected virtual void OnEnable()
        {
            m_hasHit = false;
        }

        protected virtual void Update()
        {
            if (!isPaused)
            {
                transform.position += m_speed * Time.deltaTime * transform.up;
                CheckInsideArena();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_hasHit)
                return;

            if (UtilsClass.CheckLayer(m_collisionFilter.layerMask.value, other.gameObject.layer))
                OnCollision(other);
        }

        #endregion

        public void OufOfPool(
            BulletConfig config,
            IBulletLauncher launcher,
            Vector3 position,
            Vector3 orientation)
        {
            m_speed = config.speed;
            m_damage = config.damage;

            AssignLauncher(launcher);

            transform.position = position;
            transform.up = orientation;
        }

        public virtual void AssignLauncher(IBulletLauncher launcher)
        {
            m_launcher = launcher;
        }

        protected void CheckInsideArena()
        {
            if (!LevelManager.IsInsideArena(transform.position))
                Release();
        }

        protected virtual void Release()
        {
            m_bulletChannel.onReleaseBullet.Invoke(this);
        }


        #region Collision

        protected virtual void OnCollision(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out HealthEntity healthEntity))
            {
                HitHealthEntity(healthEntity);
                ReleaseOnCollision(other);
            }
        }

        protected virtual void ReleaseOnCollision(Collider2D other)
        {
            Release();
            m_hasHit = true;

            m_bulletChannel.onDisplayImpact.Invoke(m_impactPrefab, other.ClosestPoint(transform.position));
        }

        protected virtual void HitHealthEntity(HealthEntity healthEntity)
        {
            healthEntity.LoseHealth(m_damage);
        }

        #endregion

        #region Pause

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        #endregion

        #endregion
    }
}
