using System;
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

		public EnemySettings settings;
		protected EnemyManager m_manager = null;
		protected HealthEntity m_healthEntity;
		protected bool m_isInitialized;

		[SerializeField]
		protected LevelChannel m_levelChannel = null;

		[SerializeField]
		protected EnemyType m_type = EnemyType.None;

		[SerializeField]
		private Animator m_animator;

		[SerializeField]
		private Collider2D m_collider;

		public LevelChannel levelChannel { get { return m_levelChannel; } }

		public EnemyType type { get { return m_type; } }

		public Animator animator { get { return m_animator; } }

		public new Collider2D collider { get { return m_collider; } }


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

		protected virtual void Initialize(EnemyManager manager)
		{
			m_manager = manager;
			m_healthEntity = GetComponent<HealthEntity>();
			SubscribeHealth();

			InitiliazeStates();

			m_isInitialized = true;
		}

		public virtual void OutOfPool(EnemyManager manager)
		{
			if (!m_isInitialized)
				Initialize(manager);

			m_healthEntity.maxHealth = settings.maxHealth;
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

		public void SetHittable()
		{
			collider.enabled = true;
		}

		public void SetNonHittable()
		{
			collider.enabled = false;
		}

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
			SoundManager.PlayRandomSFX(m_hitSounds);
		}

		protected virtual void CallbackNoHealth()
		{
			ChangeState(EnemyStateType.Dead);
			SoundManager.PlayRandomSFX(m_deathSounds);
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
