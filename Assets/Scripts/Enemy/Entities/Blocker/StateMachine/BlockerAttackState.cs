using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{

	public class BlockerAttackState : BlockerState
	{
		public BlockerAttackState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)EnemyStateType.Attack;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			// Look for asteroid with
		}

		public override void Update()
		{

		}
	}
}