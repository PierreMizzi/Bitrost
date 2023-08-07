using System;
using UnityEngine;

namespace Bitfrost.Gameplay.Energy
{

    [CreateAssetMenu(
    fileName = "CrystalShardsSettings",
    menuName = "Bitrost/CrystalShardsSettings",
    order = 0
)]
    public class CrystalShardsSettings : ScriptableObject
    {

        public float minDistanceBetweenCrystals;

        public float quantityToScaleRatio = 0.033f;
    }
}
