using DG.Tweening;

public class HarvesterAttackState : EnemyAttackState
{
    public HarvesterAttackState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
    }

    private Harvester m_harvester = null;

    private Tween attackTween = null;

    protected override void DefaultEnter()
    {
        base.DefaultEnter();
    }

    public void Attack()
    {
        attackTween = DOVirtual.DelayedCall(m_harvester.attackDelay, AttackComplete);
    }

    public void AttackComplete()
    {
        m_harvester.targetCrystal.DecrementEnergy();

        if (m_harvester.targetCrystal.remainingEnergyCount > 0)
        {
            Attack();
        }
        else
            ChangeState((int)EnemyStateType.Idle);
    }
}
