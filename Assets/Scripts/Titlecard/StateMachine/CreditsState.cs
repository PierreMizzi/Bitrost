namespace Bitfrost.Application
{
	using DG.Tweening;
	using PierreMizzi.Useful.StateMachines;
	using UnityEngine;

	public class CreditsState : TitlecardState
	{
		public CreditsState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)TitlecardStateType.Credits;

			m_stateTransitions.Add((int)TitlecardStateType.Menu, MenuToCreditsEnter);
		}

		private void MenuToCreditsEnter()
		{
			m_this.isInteractable = false;

			m_this.HideMenu();
			m_this.DisplayCredits();

			DOVirtual.DelayedCall(m_this.menuToCreditsDuration, () =>
			{
				m_this.isInteractable = true;
			});
		}
	}
}