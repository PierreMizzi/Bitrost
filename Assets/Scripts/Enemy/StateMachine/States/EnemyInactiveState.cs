using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{
    public class EnemyInactiveState : AState
    {
        public EnemyInactiveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)EnemyStateType.Inactive;
        }
    }
}