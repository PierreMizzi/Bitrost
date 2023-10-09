using System.Collections.Generic;
using Bitfrost.Gameplay.Enemies;
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

        [Header("Crystal Shards")]
        [SerializeField] private List<Color> m_spriteTints = new List<Color>();

        [SerializeField]
        private float m_minRotationSpeed;

        [SerializeField]
        private float m_maxRotationSpeed;

        [SerializeField]
        private float m_minCrystalScale = 1;

        [SerializeField]
        private float m_maxCrystalScale = 1.66f;

        [SerializeField]
        private float m_safeDistanceFromScaleRatio = 0.66f;

        private float sqrSafeDistanceFromScaleRatio;

        /// <summary>
        /// Distance in a square around one batch's origin position.
        /// Determines how far the crystal shards can spawn from each other
        /// </summary>
        public float randomPositionExtents;

        private void OnEnable()
        {
            sqrSafeDistanceFromScaleRatio = Mathf.Pow(m_safeDistanceFromScaleRatio, 2f);
        }

        public Color GetRandomTint()
        {
            return m_spriteTints.PickRandom();
        }

        public Vector2 GetRandomPosition()
        {
            return new Vector2(UnityEngine.Random.Range(-randomPositionExtents, randomPositionExtents),
                                UnityEngine.Random.Range(-randomPositionExtents, randomPositionExtents));
        }

        public float GetRandomRotationSpeed()
        {
            float speed = UnityEngine.Random.Range(m_minRotationSpeed, m_maxRotationSpeed);
            return speed *= UtilsClass.FlipCoin() ? 1 : -1;
        }

        public float GetRandomScale()
        {
            return UnityEngine.Random.Range(m_minCrystalScale, m_maxCrystalScale);
        }

        public float SafeDistanceFromScale(float scale)
        {
            return sqrSafeDistanceFromScaleRatio * scale;
        }

    }
}
