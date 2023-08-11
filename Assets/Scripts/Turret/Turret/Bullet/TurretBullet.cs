using UnityEngine;
using PierreMizzi.Useful;
using Bitfrost.Gameplay.Bullets;
using Bitfrost.Gameplay.Energy;

namespace Bitfrost.Gameplay.Turrets
{
    public class TurretBullet : Bullet
    {
        [Header("Turret Bullet")]
        [SerializeField]
        private TurretSettings m_settings = null;

        private CrystalShard m_originCrystal;

        public override void AssignLauncher(IBulletLauncher launcher)
        {
            base.AssignLauncher(launcher);
            Turret turret = m_launcher.gameObject.GetComponent<Turret>();
            m_originCrystal = turret.crystal;
        }

        protected override void OnCollision(Collider2D other)
        {
            base.OnCollision(other);

            if (other.gameObject.TryGetComponent(out CrystalShard crystal))
            {
                if (crystal == m_originCrystal)
                    return;

                crystal.DecrementEnergy();
                ReleaseOnCollision(other);
            }
        }
    }
}
