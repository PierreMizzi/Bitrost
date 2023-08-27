using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	public class BlockerMoveState : BlockerState
	{
		public BlockerMoveState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)EnemyStateType.Move;
		}

		private Sequence m_sequence;

		#region AState

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			m_sequence = DOTween.Sequence();
			m_sequence.Append(RotateToTarget());
			m_sequence.AppendInterval(0.2f);
			m_sequence.Append(MoveToTarget());
		}

		public override void Update()
		{
			if (isPaused)
				return;

			if (!m_this.isTargetValid)
			{
				m_this.targetTurret = null;
				ChangeState((int)EnemyStateType.Idle);
			}
		}

		public override void Exit()
		{
			if (m_sequence != null && m_sequence.IsPlaying())
				m_sequence.Kill();
		}

		#endregion

		private Tween RotateToTarget()
		{
			// float angle = Vector3.Angle(m_this.transform.up, m_directionToTarget.normalized);
			// Vector3 endDirection = new Vector3(0f, 0f, angle);
			// return m_this.transform.DORotate(endDirection, m_this.settings.moveRotationDuration);

			Vector3 fromDirection = m_this.transform.up;
			Vector3 toDirection = m_this.directionToTarget.normalized;
			return DOVirtual.Float(0, 1, m_this.settings.rotateToTargetDuration, (float value) =>
			{
				m_this.transform.up = Vector3.Lerp(fromDirection, toDirection, value);
			});
		}

		private Tween MoveToTarget()
		{
			// v= d/t | t = d/v
			float duration = m_this.directionToTarget.magnitude / m_this.settings.speed;
			Vector3 endPosition = m_this.targetTurret.transform.position + -m_this.directionToTarget.normalized * m_this.trackingRadius;

			return m_this.transform.DOMove(endPosition, duration).OnComplete(() =>
			{
				ChangeState((int)EnemyStateType.Attack);
			});
		}

		public override void Pause()
		{
			base.Pause();
			if (m_sequence != null && m_sequence.IsPlaying())
				m_sequence.Pause();
		}

		public override void Resume()
		{
			base.Resume();
			if (m_sequence != null && !m_sequence.IsPlaying())
				m_sequence.Play();
		}
	}
}