using DG.Tweening;
using UnityEngine;

public class HarvesterAttackState : EnemyAttackState
{
    public HarvesterAttackState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
    }

    private Harvester m_harvester = null;

    private Sequence attackSequence = null;

    private const string IS_ATTACKING_BOOL = "IsAttacking";

    protected override void DefaultEnter()
    {
        base.DefaultEnter();
        Attack();
    }

	public override void Exit()
	{
		base.Exit();
        m_harvester.animator.SetBool(IS_ATTACKING_BOOL, false);
        attackSequence.Kill();
	}

    public void Attack()
    {
        // attackTween = DOVirtual.DelayedCall(m_harvester.attackDelay, AttackComplete);

        attackSequence = DOTween.Sequence();
        attackSequence
            .AppendInterval(m_harvester.settings.attackDelay)
            .AppendCallback(StartAttack)
            .AppendInterval(m_harvester.settings.attackSpeed)
            .AppendCallback(CompleteAttack);
    }

    // delay
    // Attack animation
    // Stop attack animation


    public void StartAttack()
    {
        m_harvester.animator.SetBool(IS_ATTACKING_BOOL, true);
    }

    public void CompleteAttack()
    {
        m_harvester.animator.SetBool(IS_ATTACKING_BOOL, false);
        m_harvester.targetCrystal.DecrementEnergy();

        if (m_harvester.targetCrystal.remainingEnergyCount > 0)
            Attack();
        else
            ChangeState((int)EnemyStateType.Idle);
    }
}
