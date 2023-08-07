using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{
    public class EnemyMoveState : AState
    {
        public EnemyMoveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)EnemyStateType.Move;
        }
    }
}
