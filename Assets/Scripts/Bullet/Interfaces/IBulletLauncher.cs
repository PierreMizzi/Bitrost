using UnityEngine;

namespace Bitfrost.Gameplay.Bullets
{

    public interface IBulletLauncher
    {
        public GameObject gameObject { get; }

        public void Fire();

        public bool CanFire();
    }
}
