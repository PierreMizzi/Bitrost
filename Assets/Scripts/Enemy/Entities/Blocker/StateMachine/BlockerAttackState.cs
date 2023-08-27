using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	public class BlockerAttackState : BlockerState
	{
		public BlockerAttackState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)EnemyStateType.Attack;
		}

		private float m_currentAngle;
		private float m_turretAimAngle;
		private float currentVelocity;
		private Vector3 m_currentAngleDirection;

		public override void Update()
		{
			if (isPaused)
				return;

			if (m_this.isTargetValid)
				TrackTurretAim();
			else
			{
				m_this.targetTurret = null;
				ChangeState((int)EnemyStateType.Idle);
			}
		}

		private void TrackTurretAim()
		{
			// nextPosition = m_this.targetTurret.aimDirection * m_this.settings.trackingRadius;

			m_currentAngle = Mathf.Atan2(-m_this.directionToTarget.y, -m_this.directionToTarget.x) * Mathf.Rad2Deg;
			m_turretAimAngle = Mathf.Atan2(m_this.targetTurret.aimDirection.y, m_this.targetTurret.aimDirection.x) * Mathf.Rad2Deg;

			m_currentAngle = Mathf.SmoothDampAngle(m_currentAngle, m_turretAimAngle, ref currentVelocity, m_this.settings.trackingSmoothTime);
			m_currentAngleDirection = new Vector3(Mathf.Cos(m_currentAngle * Mathf.Deg2Rad), Mathf.Sin(m_currentAngle * Mathf.Deg2Rad), 0f);

			m_this.transform.position = m_this.targetTurret.transform.position + m_currentAngleDirection * m_this.trackingRadius;

			// Rotation
			m_this.transform.up = m_this.directionToTarget;
		}
	}
}