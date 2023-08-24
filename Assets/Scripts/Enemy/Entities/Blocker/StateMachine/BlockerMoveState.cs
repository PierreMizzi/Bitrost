using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{

	public class BlockerMoveState : BlockerState
	{
		public BlockerMoveState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)EnemyStateType.Move;
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