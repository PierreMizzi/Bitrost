public class FighterMoveState : EnemyMoveState
{
    public FighterMoveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
    }

	private Fighter m_fighter = null; 
}
