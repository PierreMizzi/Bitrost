using System;
using System.Collections.Generic;
using PierreMizzi.Pause;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	/// <summary>
	/// Base behaviour for all enemies
	/// </summary>
	public class Enemy : MonoBehaviour, IStateMachine, IPausable
	{
		#region Fields

		public EnemySettings settings;
		protected EnemyManager m_manager = null;
		protected HealthEntity m_healthEntity;
		public HealthEntity healthEntity => m_healthEntity;

		protected bool m_isInitialized;

		[SerializeField]
		protected LevelChannel m_levelChannel = null;

		[SerializeField]
		protected EnemyType m_type = EnemyType.None;

		[SerializeField]
		private Animator m_animator;

		[SerializeField]
		private List<Collider2D> m_colliders;

		public LevelChannel levelChannel { get { return m_levelChannel; } }

		public EnemyType type { get { return m_type; } }

		public Animator animator { get { return m_animator; } }

		public Vector3 directionTowardPlayer
		{
			get { return (m_levelChannel.player.transform.position - transform.position).normalized; }
		}

		public bool isPaused { get; set; }

		public Action onDeathAnimEnded;


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
				SoundDataID.ENEMY_DEATH_01,
				SoundDataID.ENEMY_DEATH_02,
				SoundDataID.ENEMY_DEATH_03,
				SoundDataID.ENEMY_DEATH_04,
				SoundDataID.ENEMY_DEATH_05,
				SoundDataID.ENEMY_DEATH_06,
				SoundDataID.ENEMY_DEATH_07,
				SoundDataID.ENEMY_DEATH_08,
			};

			onDeathAnimEnded = () => { };
		}

		protected virtual void Update()
		{
			UpdateState();
		}

		protected virtual void OnDestroy()
		{
			if (m_healthEntity != null)
			{
				m_healthEntity.onLostHealth -= CallbackLostHealth;
				m_healthEntity.onNoHealth -= CallbackNoHealth;
			}
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

		/// <summary>
		/// The first time an enemy leaves the pool, it means it just got instantiated and needs to be initialized
		/// </summary>
		/// <param name="manager"></param>
		protected virtual void Initialize(EnemyManager manager)
		{
			m_manager = manager;

			// health
			m_healthEntity = GetComponent<HealthEntity>();
			m_healthEntity.maxHealth = settings.maxHealth;
			if (m_healthEntity != null)
			{
				m_healthEntity.onLostHealth += CallbackLostHealth;
				m_healthEntity.onNoHealth += CallbackNoHealth;
			}

			/// State Machine
			InitiliazeStates();

			m_isInitialized = true;
		}

		/// <summary>
		/// Enemy spawned and left the pool, needs to be reset before being active
		/// </summary>
		/// <param name="manager"></param>
		public virtual void OutOfPool(EnemyManager manager)
		{
			if (!m_isInitialized)
				Initialize(manager);

			m_healthEntity.Reset();
			SetHittable();
			ChangeState(EnemyStateType.Idle);
		}

		public virtual void ReleaseToPool()
		{
			ChangeState(EnemyStateType.Inactive);
			m_manager.ReleaseToPool(this);
		}

		#endregion

		#region Health

		/// <summary>
		/// Needs to be hittable when active
		/// </summary>
		public virtual void SetHittable()
		{
			foreach (Collider2D collider in m_colliders)
				collider.enabled = true;
		}

		/// <summary>
		/// Needs to be non-hittable when dying or inactive
		/// </summary>
		public virtual void SetNonHittable()
		{
			foreach (Collider2D collider in m_colliders)
				collider.enabled = false;
		}

		protected virtual void CallbackLostHealth()
		{
			SoundManager.PlayRandomSFX(m_hitSounds);
		}

		protected virtual void CallbackNoHealth()
		{
			ChangeState(EnemyStateType.Dead);
			SoundManager.PlayRandomSFX(m_deathSounds);
		}

		#endregion

		#region Utils

		/// <summary>
		/// Get a random position around the player, rotated by current directionTowardPlayer by angle degrees.
		/// </summary>
		/// <param name="radius">radius raound enemy</param>
		/// <param name="angle">random angle</param>
		/// <returns></returns>
		public Vector3 RandomPositionAroundPlayer(float radius, float angle)
		{

			Vector3 playerPosition = m_levelChannel.player.transform.position;
			Vector3 playerToEnemy = RandomDirectionAroundPlayer(angle);

			return playerPosition + (playerToEnemy.normalized * radius);
		}

		/// <summary>
		/// Get a random direction from Player to this enemy, rotated by current directionTowardPlayer by angle degrees.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public Vector3 RandomDirectionAroundPlayer(float angle)
		{
			float randomAngle = UnityEngine.Random.Range(
				 -angle,
				 angle
			 );
			return Quaternion.AngleAxis(randomAngle, Vector3.forward) * -directionTowardPlayer;
		}

		#endregion

		#region Death

		public virtual void CallbackDeathAnimaEnd()
		{
			onDeathAnimEnded.Invoke();
		}

		#endregion

		#region Pause

		public virtual void Pause()
		{
			isPaused = true;

			currentState.Pause();
			animator.speed = 0;
		}

		public virtual void Resume()
		{
			isPaused = false;

			currentState.Resume();
			animator.speed = 1;
		}

		#endregion

		#endregion
	}
}
