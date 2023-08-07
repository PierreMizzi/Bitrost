using Bitfrost.Gameplay.Bullets;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{

    [CreateAssetMenu(fileName = "TurretSettings", menuName = "Bitrost/Gameplay/TurretSettings", order = 0)]
    public class TurretSettings : ScriptableObject
    {
        [Header("Turret Manager")]
        public int startingTurretCount = 1;

        [Header("Turret")]
        [Header("Energy")]
        public int maxStoredEnergy = 5;
        public float productionDuration = 3;
        public int productionRatio = 2;

        [Header("Bullet")]
        public Bullet bulletPrefab = null;
        public float bulletSpeed;
        public float bulletDamage = 50;
    }

}