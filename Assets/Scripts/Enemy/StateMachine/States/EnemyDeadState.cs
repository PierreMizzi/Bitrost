using System;
using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{

	public class EnemyDeadState : AState
	{
		public EnemyDeadState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)EnemyStateType.Dead;
			m_enemy = stateMachine.gameObject.GetComponent<Enemy>();
			m_enemy.onDeathAnimEnded += CallbackDeathAnimaEnd;
		}

		private Enemy m_enemy;

		private const string k_isDead = "IsDead";

		protected override void DefaultEnter()
		{
			m_enemy.animator.SetBool(k_isDead, true);
			m_enemy.SetNonHittable();
		}

		public override void Exit()
		{
			m_enemy.animator.SetBool(k_isDead, false);
			m_enemy.SetHittable();
		}

		private void CallbackDeathAnimaEnd()
		{
			m_enemy.ReleaseToPool();
		}

	}
}