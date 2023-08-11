using UnityEngine;
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

        #region Screen Edge Info

        [Header("Screen Edge Info")]
        [SerializeField]
        private TurretScreenEdgeSubject m_screenEdgeSubject = null;

        #endregion

        #region Audio

        [Header("Audio")]
        [SerializeField]
        private SoundSource m_fireBulletSource = null;
        [SerializeField]
        private SoundSource m_wrongActionSource = null;

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

        protected void Start()
        {
            m_fireBulletSource.SetSoundData(SoundDataID.TURRET_BULLET);
            m_fireBulletSource.stopOnAudioClipEnded = false;
            m_fireBulletSource.destroyOnAudioClipEnded = false;

            m_wrongActionSource.SetSoundData(SoundDataID.TURRET_WRONG_ACTION);
            m_wrongActionSource.stopOnAudioClipEnded = false;
            m_wrongActionSource.destroyOnAudioClipEnded = false;
        }

        public void Update()
        {
            UpdateState();
        }

        #endregion

        #region Activation

        public void Drop(CrystalShard crystal)
        {
            AssignCrystal(crystal);
            ChangeState(TurretStateType.Offensive);
            SoundManager.PlaySFX(SoundDataID.TURRET_DROP);
        }

        public void SetActive()
        {
            m_target.gameObject.SetActive(true);
            m_spritesContainer.SetActive(true);
            m_screenEdgeSubject.gameObject.SetActive(true);

            onRefreshEnergy.Invoke();
        }

        public void Retrieve()
        {
            storedEnergy = 0;
            onRefreshEnergy.Invoke();
            RemoveCrystal();

            ChangeState(TurretStateType.Inactive);
            SoundManager.PlaySFX(SoundDataID.TURRET_RETRIEVE);
        }

        public void SetInactive()
        {
            m_target.gameObject.SetActive(false);
            m_spritesContainer.SetActive(false);
            m_screenEdgeSubject.gameObject.SetActive(false);
        }

        public void AssignCrystal(CrystalShard crystal)
        {
            this.crystal = crystal;
            this.crystal.SetOccupied();

            transform.position = this.crystal.transform.position;

            onAssignCrystal.Invoke();
            this.crystal.onRefreshEnergy.Invoke();
        }

        public void RemoveCrystal()
        {
            crystal.SetUnoccupied();

            if (!crystal.hasEnergy)
                crystal.ReleaseToPool();

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

                m_fireBulletSource.Play();
            }
            else
                m_wrongActionSource.Play();

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

        #region Sounds Sources



        #endregion

        #endregion
    }
}
