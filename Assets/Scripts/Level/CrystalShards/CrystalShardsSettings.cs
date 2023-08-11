using System;
using PierreMizzi.Useful;
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

        [Header("Crystal Shards Manager")]
        public float minDistanceBetweenCrystals;

        [Header("Crystal Shards")]
        [SerializeField]
        private float m_minRotationSpeed;

        [SerializeField]
        private float m_maxRotationSpeed;

        [SerializeField]
        private float m_minCrystalScale = 1;

        [SerializeField]
        private float m_maxCrystalScale = 1.66f;


        public float GetRandomRotationSpeed()
        {
            float speed = UnityEngine.Random.Range(m_minRotationSpeed, m_maxRotationSpeed);
            return speed *= UtilsClass.FlipCoin() ? 1 : -1;
        }

        public float GetRandomScale()
        {
            return UnityEngine.Random.Range(m_minCrystalScale, m_maxCrystalScale);
        }

    }
}
