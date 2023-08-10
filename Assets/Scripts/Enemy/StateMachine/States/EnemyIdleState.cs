using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{
    public class EnemyIdleState : AState
    {
        public EnemyIdleState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)EnemyStateType.Idle;
        }

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            ChangeState((int)EnemyStateType.Move);
        }
    }
}
