using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Turrets
{
    public class ATurretState : AState
    {
        public ATurretState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_turret = stateMachine.gameObject.GetComponent<Turret>();
        }

        protected Turret m_turret;

        public void ChangeState(TurretStateType state)
        {
            ChangeState((int)state);
        }
    }
}
