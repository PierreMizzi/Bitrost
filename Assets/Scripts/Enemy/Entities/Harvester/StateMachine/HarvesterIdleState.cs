using System.Collections;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	public class HarvesterIdleState : EnemyIdleState
	{
		public HarvesterIdleState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
		}

		private Harvester m_harvester;

		private IEnumerator m_searchTargetCoroutine;

		protected override void DefaultEnter()
		{
			StartSearchingTarget();
		}

		public override void Exit()
		{
			StopSeachingTarget();
		}

		private void StartSearchingTarget()
		{
			if (m_searchTargetCoroutine == null)
			{
				m_searchTargetCoroutine = SearchTargetCrystalCoroutine();
				m_harvester.StartCoroutine(m_searchTargetCoroutine);
			}
		}

		private void StopSeachingTarget()
		{
			if (m_searchTargetCoroutine != null)
			{
				m_harvester.StopCoroutine(m_searchTargetCoroutine);
				m_searchTargetCoroutine = null;
			}
		}

		private IEnumerator SearchTargetCrystalCoroutine()
		{
			while (true)
			{
				m_harvester.SearchTargetCrystal();

				if (m_harvester.targetCrystal != null)
					ChangeState((int)EnemyStateType.Move);

				yield return new WaitForSeconds(1f);
			}
		}

	}

}