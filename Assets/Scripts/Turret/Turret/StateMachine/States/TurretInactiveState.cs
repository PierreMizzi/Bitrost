using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Turrets
{

    public class TurretInactiveState : ATurretState
    {
        public TurretInactiveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)TurretStateType.Inactive;
        }

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            m_this.SetInactive();
        }
    }
}
