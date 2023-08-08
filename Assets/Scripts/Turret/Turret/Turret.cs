using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;
using PierreMizzi.SoundManager;
using Bitfrost.Gameplay.Bullets;
using Bitfrost.Gameplay.Energy;

namespace Bitfrost.Gameplay.Turrets
{

    public delegate void TurretDelegate(Turret turret);
    public delegate void TurretStateDelegate(TurretStateType type);

    // TODO : Small message when can't fire because extract mode
    public partial class Turret : MonoBehaviour, IBulletLauncher, IPausable
    {
        #region Fields

        #region Behaviour

        [Header("Channels")]
        [SerializeField]
        private BulletChannel m_bulletChannel;

        [SerializeField]
        private LevelChannel m_levelChannel;

        [Header("Behaviour")]
        [SerializeField]
        private TurretSettings m_settings;

        public TurretSettings settings
        {
            get { return m_settings; }
        }

        public CrystalShard crystal { get; private set; }

        [SerializeField]
        private TurretTarget m_target;

        [SerializeField]
        private GameObject m_spritesContainer = null;

        [SerializeField]
        private GameObject m_aimSprite;
        private bool m_isDroppable;
        private bool m_isTargeted = false;

        public GameObject aimSprite { get { return m_aimSprite; } }

        public bool isDroppable
        {
            get { return m_isDroppable; }
            set
            {
                m_isDroppable = value;
                onIsDroppable?.Invoke();
            }
        }
        public bool isTargeted
        {
            get { return m_isTargeted; }
            set
            {
                m_isTargeted = value;
                onIsTargeted?.Invoke();
            }
        }

        public bool isPaused { get; set; }

        #endregion

        #region Offensive

        [Header("Offensive")]
        [SerializeField]
        private InputActionReference m_mousePositionInput = null;

        [SerializeField]
        private Transform m_canonTransform;

        [SerializeField]
        private Transform m_bulletOriginTransform;

        private Vector3 m_mouseWorldSpace;

        public new Camera camera { get; private set; }

        public Vector3 aimDirection { get; private set; }

        public Transform canonTransform
        {
            get { return m_canonTransform; }
        }

        #endregion

        #region Production

        private float m_productionProgress;
        public float productionProgress
        {
            get { return m_productionProgress; }
            set
            {
                m_productionProgress = Mathf.Clamp01(value);
                onProductionProgress?.Invoke(m_productionProgress);
            }
        }

        public int m_storedEnergy;

        public int storedEnergy
        {
            get { return m_storedEnergy; }
            set { m_storedEnergy = Mathf.Clamp(value, 0, m_settings.maxStoredEnergy); }
        }

        private bool canStoreEnergy
        {
            get
            {
                return m_storedEnergy < m_settings.maxStoredEnergy;
            }
        }

        public bool hasEnergy
        {
            get { return crystal.hasEnergy || m_storedEnergy > 0; }
        }

        #endregion

        #region UI events
        // Keep
        public Action onIsDroppable = null;
        public Action onIsTargeted = null;

        public TurretStateDelegate onChangeState = null;

        public Action onAssignCrystal = null;
        public Action onRemoveCrystal = null;
        public Action onRefreshEnergy = null;

        public FloatDelegate onProductionProgress = null;

        #endregion

        #endregion

        #region Methods

        public void Initialize(TurretManager manager)
        {
            camera = Camera.main;

            storedEnergy = 0;

            ChangeState(TurretStateType.Inactive);
        }

        #region MonoBehaviour

        protected void Awake()
        {
            camera = Camera.main;

            InitiliazeStates();

            onIsDroppable = () => { };
            onIsTargeted = () => { };

            onChangeState = (TurretStateType state) => { };

            onAssignCrystal = () => { };
            onRemoveCrystal = () => { };
            onRefreshEnergy = () => { };

            onProductionProgress = (float normalized) => { };
        }

        public void Update()
        {
            UpdateState();
        }

        #endregion

        #region Activation

        public void SetActive()
        {
            m_target.gameObject.SetActive(true);
            m_spritesContainer.SetActive(true);

            onRefreshEnergy.Invoke();
        }

        public void SetInactive()
        {
            m_target.gameObject.SetActive(false);
            m_spritesContainer.SetActive(false);
        }

        public void AssignCrystal(CrystalShard crystal)
        {
            this.crystal = crystal;
            this.crystal.SetUnavailable();

            transform.position = this.crystal.transform.position;

            onAssignCrystal.Invoke();
            this.crystal.onRefreshEnergy.Invoke();
        }

        public void RemoveCrystal()
        {
            crystal.SetAvailable();

            if (!crystal.hasEnergy)
                crystal.Destroy();

            onRemoveCrystal.Invoke();
            crystal = null;
        }

        #endregion

        #region Fire

        public void ComputeAimDirection()
        {
            m_mouseWorldSpace = m_mousePositionInput.action.ReadValue<Vector2>();
            m_mouseWorldSpace = camera.ScreenToWorldPoint(m_mouseWorldSpace);
            m_mouseWorldSpace.z = 0;

            aimDirection = (m_mouseWorldSpace - transform.position).normalized;
        }

        public void Fire()
        {
            if (CanFire())
            {
                if (storedEnergy > 0)
                    storedEnergy--;
                else
                    crystal.DecrementEnergy();

                onRefreshEnergy();

                m_bulletChannel.onInstantiateBullet.Invoke(
                    this,
                    m_settings.bulletPrefab,
                    m_bulletOriginTransform.position,
                    aimDirection
                );

                if (!hasEnergy)
                    ChangeState(TurretStateType.Disabled);

                SoundManager.PlaySound(SoundDataIDStatic.TURRET_BULLET);

            }
        }

        public bool CanFire()
        {
            bool result = true;

            bool inOffensiveState = (TurretStateType)currentState.type == TurretStateType.Offensive;
            result &= inOffensiveState;

            if (!inOffensiveState)
                Debug.LogWarning("IS NOT IN OFFENSIVE STATE");

            return result;
        }

        #endregion

        #region Pause


        public void Pause()
        {
            isPaused = true;
            currentState.Pause();
        }

        public void Resume()
        {
            isPaused = false;
            currentState.Resume();
        }

        #endregion

        #endregion
    }
}