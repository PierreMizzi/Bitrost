using System.Collections;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	/// <summary>
	/// When no target crystal has been found, the blocker rotates around itself, looking for an occupied crystal shard
	/// </summary>
	public class BlockerIdleState : BlockerState
	{
		public BlockerIdleState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)EnemyStateType.Idle;
		}

		private Vector3 m_orbitAxis;

		// 1 = clockwise, -1 = anti-clockwise 
		private float m_orbitDirection = 1;
		private float m_orbitAngleDelta;

		private IEnumerator m_searchTargetCoroutine;

		#region AState

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			m_orbitDirection = UtilsClass.FlipCoin() ? 1 : -1;
			m_orbitAxis = m_this.transform.position + m_this.transform.up * m_this.settings.idleOrbitRadius;
			StartSearchingTarget();
		}

		public override void Update()
		{
			if (isPaused)
				return;

			// Position
			m_orbitAngleDelta = Time.deltaTime * m_this.settings.idleOrbitSpeed * m_orbitDirection;
			m_this.transform.RotateAround(m_orbitAxis, Vector3.forward, m_orbitAngleDelta);

			// Rotation
			m_this.transform.up = (m_orbitAxis - m_this.transform.position).normalized;
		}

		public override void Exit()
		{
			StopSeachingTarget();
		}

		#endregion


		#region Search for target

		private void StartSearchingTarget()
		{
			if (m_searchTargetCoroutine == null)
			{
				m_searchTargetCoroutine = SearchTargetCrystalCoroutine();
				m_this.StartCoroutine(m_searchTargetCoroutine);
			}
		}

		private void StopSeachingTarget()
		{
			if (m_searchTargetCoroutine != null)
			{
				m_this.StopCoroutine(m_searchTargetCoroutine);
				m_searchTargetCoroutine = null;
			}
		}

		private IEnumerator SearchTargetCrystalCoroutine()
		{
			while (true)
			{
				m_this.SearchTargetCrystal();

				if (m_this.targetTurret != null)
					ChangeState((int)EnemyStateType.Move);

				yield return new WaitForSeconds(1f);
			}
		}

		#endregion

	}
}