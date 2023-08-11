using Bitfrost.Gameplay.Bullets;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    [CreateAssetMenu(
    fileName = "FighterSettings",
    menuName = "Bitrost/Enemies/FighterSettings",
    order = 0
)]
    public class FighterSettings : EnemySettings
    {
        [Header("Movement")]
        public float radiusAroundPlayer;

        public float angleAroundPlayer;

        [Header("Attack")]
        public BulletConfig bulletConfig;

        /// <summary> 
        /// Amount of bullets in a salvo
        /// </summary>
        public int bulletSalvoCount = 3;

        /// <summary>
        ///	Delay between each bullets in a salvo
        /// </summary>
        public float bulletSalvoRateOfFire = 0.25f;

        /// <summary> 
        /// Delay between two salvos
        /// </summary>
        public float delayBetweenSalvo = 4f;
    }
}
