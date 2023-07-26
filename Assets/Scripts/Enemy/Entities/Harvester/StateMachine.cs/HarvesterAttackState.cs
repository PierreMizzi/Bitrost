public class HarvesterAttackState : EnemyAttackState
{
    public HarvesterAttackState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
    }

    private Harvester m_harvester = null;
}
