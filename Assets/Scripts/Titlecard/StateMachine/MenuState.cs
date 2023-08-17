namespace Bitfrost.Application
{
	using System;
	using DG.Tweening;
	using PierreMizzi.Useful.StateMachines;
	using UnityEngine;

	public class MenuState : TitlecardState
	{
		public MenuState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)TitlecardStateType.Menu;

			m_stateTransitions.Add((int)TitlecardStateType.Credits, CreditsToMenuEnter);
		}

		protected override void DefaultEnter()
		{
			m_this.isInteractable = true;
		}

		private void CreditsToMenuEnter()
		{
			m_this.isInteractable = false;

			m_this.DisplayMenu();
			m_this.HideCredits();

			DOVirtual.DelayedCall(m_this.creditsToMenuDuration, () =>
			{
				m_this.isInteractable = true;
			});
		}



	}
}