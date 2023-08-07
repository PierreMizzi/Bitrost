using System;
using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	public class ScoutAttackState : AScoutState
	{
		public ScoutAttackState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)EnemyStateType.Attack;
		}

		private Vector3 m_velocity;

		private Tween m_attackTween;

		protected override void DefaultEnter()
		{
			StartAttacking();
		}

		public override void Exit()
		{
			if (m_attackTween != null && m_attackTween.IsPlaying())
				m_attackTween.Kill();
		}

		public override void Update()
		{
			m_this.transform.position = Vector3.SmoothDamp(m_this.transform.position, m_this.positionAroundPlayer, ref m_velocity, m_this.speedTrackPlayer);
			m_this.transform.up = m_this.directionTowardPlayer;
		}

		private void StartAttacking()
		{
			m_attackTween = DOVirtual.DelayedCall(m_this.settings.delayBetweenBullet, m_this.Fire)
									 .SetLoops(-1);
		}

		public override void Pause()
		{
			if (m_attackTween != null && m_attackTween.IsPlaying())
				m_attackTween.Pause();
		}

		public override void Resume()
		{
			if (m_attackTween != null && !m_attackTween.IsPlaying())
				m_attackTween.Play();
		}

	}
}
