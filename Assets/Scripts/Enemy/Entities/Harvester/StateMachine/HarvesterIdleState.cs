using DG.Tweening;
using PierreMizzi.Useful.StateMachines;

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

		private Tween m_delayTween;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			SearchForCrystal();

			m_delayTween = DOVirtual.DelayedCall(
				1f,
				() =>
				{
					SearchForCrystal();
				}
			).SetLoops(-1);
		}

		protected void SearchForCrystal()
		{
			m_harvester.SearchCrystalShard();

			if (m_harvester.targetCrystal != null)
				ChangeState((int)EnemyStateType.Move);
		}
	}

}