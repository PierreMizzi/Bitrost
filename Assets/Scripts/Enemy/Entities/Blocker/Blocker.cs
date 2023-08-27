using System;
using System.Collections.Generic;
using Bitfrost.Gameplay.Energy;
using Bitfrost.Gameplay.Turrets;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	public class Blocker : Enemy
	{
		#region Fields 

		public new BlockerSettings settings
		{
			get { return base.settings as BlockerSettings; }
		}

		#region WeakSpots

		[SerializeField]
		private List<WeakSpot> m_weakSpots = new List<WeakSpot>();

		private bool allWeakSpotsDestroyed
		{
			get
			{
				foreach (WeakSpot weakSpot in m_weakSpots)
				{
					if (!weakSpot.isDestroyed)
						return false;
				}
				return true;
			}
		}

		#endregion

		#region Attack

		private Predicate<CrystalShard> m_targetableCrystalPredicate;

		public Turret targetTurret { get; set; }

		public bool isTargetValid
		{
			get
			{
				return targetTurret != null &&
					   targetTurret.currentState.type != (int)TurretStateType.Inactive;
			}
		}

		public Vector3 directionToTarget
		{
			get
			{
				if (targetTurret != null)
					return targetTurret.transform.position - transform.position;
				else
					return Vector3.zero;
			}
		}

		public float trackingRadius { get; private set; }

		#endregion

		#endregion

		#region Methods 

		#region MonoBehaviour

		protected override void Awake()
		{
			base.Awake();
			m_targetableCrystalPredicate = (CrystalShard crystal) => crystal != null;
		}

		protected virtual void Start()
		{
			foreach (WeakSpot weakSpot in m_weakSpots)
				weakSpot.onDestroyed += CallbackWeakSpotDestroyed;
		}

		#endregion

		#region Behaviour

		protected override void Initialize(EnemyManager manager)
		{
			m_manager = manager;
			InitiliazeStates();
			InitializeWeakSpots();

			m_isInitialized = true;
		}

		public override void InitiliazeStates()
		{
			states = new List<AState>()
			{
				new EnemyInactiveState(this),
				new EnemyDeadState(this),
				new BlockerIdleState(this),
				new BlockerMoveState(this),
				new BlockerAttackState(this),
			};
		}

		public override void OutOfPool(EnemyManager manager)
		{
			if (!m_isInitialized)
				Initialize(manager);

			ResetWeakSpots();

			SetHittable();
			ChangeState(EnemyStateType.Idle);
		}

		public override void SetHittable()
		{
			base.SetHittable();

			foreach (WeakSpot weakSpot in m_weakSpots)
				weakSpot.SetHittable();
		}

		public override void SetNonHittable()
		{
			base.SetNonHittable();

			foreach (WeakSpot weakSpot in m_weakSpots)
				weakSpot.SetNonHittable();
		}

		#endregion

		#region WeakSpots

		private void InitializeWeakSpots()
		{
			foreach (WeakSpot weakSpot in m_weakSpots)
				weakSpot.Initialize(settings.weakSpotMaxHealth);
		}

		private void ResetWeakSpots()
		{
			foreach (WeakSpot weakSpot in m_weakSpots)
				weakSpot.Reset();
		}

		private void CallbackWeakSpotDestroyed()
		{
			if (allWeakSpotsDestroyed)
				ChangeState(EnemyStateType.Dead);
		}

		#endregion

		#region Attack

		public void SearchTargetCrystal()
		{
			CrystalShard crystal = m_levelChannel.crystalManager.PickRandomOccupiedCrystal(m_targetableCrystalPredicate);

			if (crystal != null)
			{
				trackingRadius = settings.trackingRadius * crystal.transform.localScale.x;
				targetTurret = crystal.turret;
			}
		}



		#endregion

		#endregion
	}
}