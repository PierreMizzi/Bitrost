using System;
using UnityEngine;
using DG.Tweening;
using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{
	public class ScoutMoveState : AScoutState
	{
		public ScoutMoveState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)EnemyStateType.Move;
		}

		private Tween m_approachPlayerTween;

		protected override void DefaultEnter()
		{
			ApproachPlayer();
		}

		public override void Update()
		{
			m_this.transform.up = m_this.directionTowardPlayer;
		}

		public override void Exit()
		{
			if (m_approachPlayerTween != null && m_approachPlayerTween.IsPlaying())
				m_approachPlayerTween.Kill();
		}

		private void ApproachPlayer()
		{
			float distance = (m_this.positionAroundPlayer - m_this.transform.position).magnitude;
			float duration = distance / m_this.settings.speed;

			m_approachPlayerTween = m_this.transform.DOMove(m_this.positionAroundPlayer, duration)
													.OnComplete(ApproachPlayerComplete)
													.SetEase(Ease.Linear);

		}

		private void ApproachPlayerComplete()
		{
			ChangeState((int)EnemyStateType.Attack);
		}

		public override void Pause()
		{
			base.Pause();

			if (m_approachPlayerTween != null && m_approachPlayerTween.IsPlaying())
				m_approachPlayerTween.Pause();
		}

		public override void Resume()
		{
			base.Resume();

			if (m_approachPlayerTween != null && !m_approachPlayerTween.IsPlaying())
				m_approachPlayerTween.Play();
		}

	}
}
