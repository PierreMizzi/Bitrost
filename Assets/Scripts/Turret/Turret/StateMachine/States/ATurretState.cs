using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Turrets
{
    public class ATurretState : AState
    {
        public ATurretState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_this = stateMachine.gameObject.GetComponent<Turret>();
        }

        protected Turret m_this;

        public void ChangeState(TurretStateType state)
        {
            ChangeState((int)state);
        }

        protected override void DefaultEnter()
        {
            m_this.manager.ManageCursor();
        }
    }
}
