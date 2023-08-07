using System;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    [Serializable]
    public struct EnemySpawnShortcutConfig
    {
        public Enemy prefab;

        public KeyCode quickSpawnKey;
    }
}
