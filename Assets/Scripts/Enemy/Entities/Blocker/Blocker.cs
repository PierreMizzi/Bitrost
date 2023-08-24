using System;
using System.Collections.Generic;
using Bitfrost.Gameplay.Energy;
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

		public CrystalShard targetCrystal { get; private set; }

		public bool isCrystalValid
		{
			get
			{
				return targetCrystal != null && targetCrystal.hasEnergy;
			}
		}

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
				new EnemyIdleState(this),
				new EnemyDeadState(this),
				new BlockerAttackState(this),
				new BlockerMoveState(this),
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

		private void SearchTargetCrystal()
		{
			targetCrystal = m_levelChannel.crystalManager.PickRandomOccupiedCrystal(m_targetableCrystalPredicate);
		}

		#endregion

		#endregion
	}
}