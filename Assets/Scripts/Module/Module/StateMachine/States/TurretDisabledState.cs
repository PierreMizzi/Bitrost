using UnityEngine;

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
        m_turret.canonTransform.up = Vector2.up;
    }
}
