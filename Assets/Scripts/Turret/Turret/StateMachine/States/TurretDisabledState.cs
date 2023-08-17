using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{

    public class TurretDisabledState : ATurretState
    {
        public TurretDisabledState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)TurretStateType.Disabled;
        }

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            m_this.ChangeAnimatorState(type);
        }
    }
}
