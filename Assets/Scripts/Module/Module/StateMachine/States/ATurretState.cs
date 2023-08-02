public class ATurretState : AState
{
    public ATurretState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_turret = stateMachine.gameObject.GetComponent<Module>();
    }

    protected Module m_turret;
}
