public class TurretInactiveState : ATurretState
{
    public TurretInactiveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        type = (int)TurretStateType.Inactive;
    }

    protected override void DefaultEnter()
    {
		m_turret.SetInactive();
    }
}
