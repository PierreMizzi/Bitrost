using System.Collections.Generic;
using UnityEngine;

namespace Bitfrost.Gameplay.Bullets
{

    [CreateAssetMenu(fileName = "BulletChannel", menuName = "Bitrost/Bullet/BulletChannel", order = 0)]
    public class BulletChannel : ScriptableObject
    {
        public InstantiateBulletDelegate onInstantiateBullet = null;

        public ReleaseBulletDelegate onReleaseBullet = null;

        public List<BulletPoolConfig> bulletPoolConfigs = new List<BulletPoolConfig>();

        private void OnEnable()
        {
            onInstantiateBullet = (
                IBulletLauncher launcher,
                Bullet prefab,
                Vector3 position,
                Vector3 orientation
            ) =>
            { };
            onReleaseBullet = (Bullet bullet) => { };
        }
    }
}
