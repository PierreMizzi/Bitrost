using System.Collections.Generic;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    [RequireComponent(typeof(HealthEntity))]
    public class Enemy : MonoBehaviour, IStateMachine, IPausable
    {
        #region Fields

        [SerializeField]
        protected LevelChannel m_levelChannel = null;

        public LevelChannel levelChannel
        {
            get { return m_levelChannel; }
        }

        public Vector3 directionTowardPlayer
        {
            get { return (m_levelChannel.player.transform.position - transform.position).normalized; }
        }

        [SerializeField]
        protected EnemyType m_type = EnemyType.None;
        public EnemyType type
        {
            get { return m_type; }
        }

        protected HealthEntity m_healthEntity;
        protected bool m_isInitialized;
        protected EnemyManager m_manager = null;

        public EnemySettings settings;

        public bool isPaused { get; set; }


        #region StateMachine

        public List<AState> states { get; set; } = new List<AState>();
        public AState currentState { get; set; }

        #endregion

        #region Sounds

        protected List<string> m_deathSounds = new List<string>();

        protected List<string> m_hitSounds = new List<string>();

        #endregion

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            m_deathSounds = new List<string>(){
            SoundDataIDStatic.ENEMY_DEATH_01,
            SoundDataIDStatic.ENEMY_DEATH_02,
            SoundDataIDStatic.ENEMY_DEATH_03,
            SoundDataIDStatic.ENEMY_DEATH_04,
            SoundDataIDStatic.ENEMY_DEATH_05,
            SoundDataIDStatic.ENEMY_DEATH_06,
            SoundDataIDStatic.ENEMY_DEATH_07,
            SoundDataIDStatic.ENEMY_DEATH_08,
        };
        }

        protected virtual void Update()
        {
            UpdateState();
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeHealth();
        }

        #region StateMachine

        public virtual void InitiliazeStates()
        {
            states = new List<AState>()
        {
            new EnemyIdleState(this),
            new EnemyMoveState(this),
            new EnemyAttackState(this)
        };
        }

        public void ChangeState(EnemyStateType nextState, EnemyStateType previousState = EnemyStateType.None)
        {
            ChangeState((int)previousState, (int)nextState);
        }

        public void ChangeState(int previousState, int nextState)
        {
            currentState?.Exit();

            currentState = states.Find((AState newState) => newState.type == nextState);
            if (currentState != null)
                currentState.Enter(previousState);
            else
            {
                Debug.LogError($"Couldn't find a new state of type : {nextState}. Going Inactive");
            }
        }

        public void UpdateState()
        {
            if (!isPaused)
                currentState?.Update();
        }

        #endregion

        #region Behaviour

        protected virtual void Initialize()
        {
            m_healthEntity = GetComponent<HealthEntity>();
            SubscribeHealth();

            InitiliazeStates();

            m_isInitialized = true;
        }

        public virtual void OutOfPool(EnemyManager manager)
        {
            if (!m_isInitialized)
                Initialize();

            m_healthEntity.maxHealth = settings.maxHealth;
            m_healthEntity.Reset();
            m_manager = manager;
            ChangeState(EnemyStateType.Idle);

        }

        #endregion

        #region Health

        protected virtual void SubscribeHealth()
        {
            m_healthEntity.onLostHealth += CallbackLostHealth;
            m_healthEntity.onNoHealth += CallbackNoHealth;
        }

        protected virtual void UnsubscribeHealth()
        {
            m_healthEntity.onLostHealth -= CallbackLostHealth;
            m_healthEntity.onNoHealth -= CallbackNoHealth;
        }

        protected virtual void CallbackLostHealth()
        {
            SoundManager.PlayRandomSound(m_hitSounds);
        }

        protected virtual void CallbackNoHealth()
        {
            ChangeState(EnemyStateType.Inactive);
            m_manager.KillEnemy(this);
            SoundManager.PlayRandomSound(m_deathSounds);
        }

        #endregion

        #region Utils

        public Vector3 CloseRandomPositionAroundPlayer(float radius, float angle)
        {

            Vector3 playerPosition = m_levelChannel.player.transform.position;
            Vector3 playerToEnemy = CloseRandomDirectionFromPlayer(angle);

            return playerPosition + (playerToEnemy.normalized * radius);
        }

        public Vector3 PositionAroundPlayer(Vector3 direction, float radius)
        {
            Vector3 playerPosition = m_levelChannel.player.transform.position;

            return playerPosition + (direction * radius);
        }

        public Vector3 CloseRandomDirectionFromPlayer(float angle)
        {
            float randomAngle = Random.Range(
                 -angle,
                 angle
             );
            return Quaternion.AngleAxis(randomAngle, Vector3.forward) * -directionTowardPlayer;
        }

        #endregion

        #region Pause

        public virtual void Pause()
        {
            isPaused = true;

            currentState.Pause();
        }

        public virtual void Resume()
        {
            isPaused = false;

            currentState.Resume();
        }

        #endregion

        #endregion
    }
}
