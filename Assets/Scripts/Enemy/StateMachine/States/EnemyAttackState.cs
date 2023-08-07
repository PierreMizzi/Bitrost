using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{

    public class EnemyAttackState : AState
    {
        public EnemyAttackState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)EnemyStateType.Attack;
        }

    }
}