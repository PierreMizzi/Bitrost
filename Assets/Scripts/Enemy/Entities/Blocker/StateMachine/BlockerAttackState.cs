using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

	/// <summary>
	/// Blocker's attack state is following the turrets orientation to potentialy blocks its bullets
	/// </summary>
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
			// curret angle between Vector3.right and the line between turret and blocker (directioToTarget)
			m_currentAngle = Mathf.Atan2(-m_this.directionToTarget.y, -m_this.directionToTarget.x) * Mathf.Rad2Deg;

			// current angle between Vector3.right and turret's orientation
			m_turretAimAngle = Mathf.Atan2(m_this.targetTurret.aimDirection.y, m_this.targetTurret.aimDirection.x) * Mathf.Rad2Deg;

			// Smooth lerped currentAngle to get closer to turretAimAngle
			m_currentAngle = Mathf.SmoothDampAngle(m_currentAngle, m_turretAimAngle, ref currentVelocity, m_this.settings.trackingSmoothTime);

			// Angle to vector direction
			m_currentAngleDirection = new Vector3(Mathf.Cos(m_currentAngle * Mathf.Deg2Rad), Mathf.Sin(m_currentAngle * Mathf.Deg2Rad), 0f);

			// Set position using radius
			m_this.transform.position = m_this.targetTurret.transform.position + m_currentAngleDirection * m_this.trackingRadius;

			// Set rotation perpendicular to turret's orientation
			m_this.transform.up = m_this.directionToTarget;
		}
	}
}