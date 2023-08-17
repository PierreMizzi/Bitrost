namespace Bitfrost.Application
{
	using PierreMizzi.Useful.StateMachines;
	using UnityEngine;

	public class TitlecardState : AState
	{

		public TitlecardState(IStateMachine stateMachine) : base(stateMachine)
		{
			m_this = stateMachine.gameObject.GetComponent<TitlecardManager>();
		}

		protected TitlecardManager m_this;
	}
}