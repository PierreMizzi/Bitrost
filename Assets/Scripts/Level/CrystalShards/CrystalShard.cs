using System;
using UnityEngine;
using PierreMizzi.Rendering;
using PierreMizzi.Useful;

namespace Bitfrost.Gameplay.Energy
{
    public class CrystalShard : MonoBehaviour
    {

        #region Fields 

        private CrystalShardsManager m_manager;

        [SerializeField]
        private CrystalShardsSettings m_settings = null;

        private bool m_isInitialized;
        public bool isOccupied { get; private set; }

        #region Energy

        public int totalEnergyCount { get; private set; }

        public int remainingEnergyCount { get; private set; }

        public float energyPercentage
        {
            get { return remainingEnergyCount / (float)totalEnergyCount; }
        }

        public bool hasEnergy
        {
            get { return remainingEnergyCount > 0; }
        }

        public Action onRefreshEnergy = null;

        public Action onNoEnergy = null;

        #endregion

        #region Rendering

        [Header("Rendering")]
        [SerializeField]
        private SpriteRenderer m_spriteRenderer = null;

        private Color m_spriteRendererTint;

        public SpriteRenderer spriteRenderer { get { return m_spriteRenderer; } }

        [SerializeField]
        private MaterialPropertyBlockModifier m_propertyBlock = null;

        private const string k_energyPercentProperty = "_EnergyPercent";
        private const string k_noiseOffsetProperty = "_MarbleNoiseOffset";

        [SerializeField]
        private float m_rotationSpeed;

        #endregion

        #endregion

        #region Methods 

        #region MonoBehaviour

        protected void Awake()
        {
            onRefreshEnergy = () => { };

        }

        protected void Update()
        {
            if (!isOccupied)
                m_spriteRenderer.transform.rotation *= Quaternion.Euler(0, 0, m_rotationSpeed * Time.deltaTime);
        }

        #endregion

        public void OutOfPool(CrystalShardsManager manager, int energyCount)
        {
            if (!m_isInitialized)
                Initialize(manager);

            totalEnergyCount = energyCount;
            remainingEnergyCount = energyCount;
            SetVisualEnergy();

            m_propertyBlock.SetProperty(UtilsClass.MainTexProperty, m_settings.GetRandomSprite());
            m_spriteRenderer.color = m_settings.GetRandomTint();
            float noiseOffset = UnityEngine.Random.Range(0f, 100f);
            m_propertyBlock.SetProperty(k_noiseOffsetProperty, noiseOffset);

            m_rotationSpeed = m_settings.GetRandomRotationSpeed();
        }

        public void Initialize(CrystalShardsManager manager)
        {
            m_manager = manager;
            m_isInitialized = true;

        }

        public void ReleaseToPool()
        {
            Reset();
            m_manager.ReleaseCrystal(this);
        }

        public void SetOccupied()
        {
            isOccupied = true;
            m_manager.AddOccupiedCrystal(this);
        }

        public void SetUnoccupied()
        {
            isOccupied = false;
            m_manager.RemoveOccupiedCrystal(this);
        }

        public void DecrementEnergy()
        {
            remainingEnergyCount--;

            SetVisualEnergy();
            onRefreshEnergy.Invoke();
        }

        private void SetVisualEnergy()
        {
            m_propertyBlock.SetProperty(k_energyPercentProperty, energyPercentage);
        }

        public void Reset()
        {
            remainingEnergyCount = 0;
            totalEnergyCount = 0;
            isOccupied = false;

            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
        }

        public void Hide()
        {
            m_spriteRendererTint = m_spriteRenderer.color;
            m_spriteRendererTint.a = 0;
            m_spriteRenderer.color = m_spriteRendererTint;
        }

        #endregion
    }


}
