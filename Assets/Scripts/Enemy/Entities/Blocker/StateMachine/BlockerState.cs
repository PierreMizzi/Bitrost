using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{

	public class BlockerState : AState
	{
		public BlockerState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<Blocker>();
		}

		protected Blocker m_this;
	}
}