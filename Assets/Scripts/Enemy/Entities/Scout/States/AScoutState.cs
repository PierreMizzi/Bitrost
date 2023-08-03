public class AScoutState : AState
{
	public AScoutState(IStateMachine stateMachine) : base(stateMachine)
	{
		m_this = stateMachine.gameObject.GetComponent<Scout>();
	}

	protected Scout m_this;
}