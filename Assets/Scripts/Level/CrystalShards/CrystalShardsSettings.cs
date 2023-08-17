using System.Collections.Generic;
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

        [SerializeField] private List<Texture> m_sprites = new List<Texture>();

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

        private float safeDistanceFromScaleRatio;

        public float randomPositionExtents;

        private void OnEnable()
        {
            safeDistanceFromScaleRatio = Mathf.Pow(m_safeDistanceFromScaleRatio, 2f);
        }

        public Texture GetRandomSprite()
        {
            return UtilsClass.PickRandom(m_sprites);
        }

        public Color GetRandomTint()
        {
            return UtilsClass.PickRandom(m_spriteTints);
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
            return safeDistanceFromScaleRatio * scale;
        }

    }
}
