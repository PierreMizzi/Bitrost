using DG.Tweening;
using UnityEngine;

public class FighterAttackState : EnemyAttackState
{
    public FighterAttackState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
    }

    private Fighter m_fighter = null;

    private Tween m_attackTween = null;

    protected override void DefaultEnter()
    {
        base.DefaultEnter();

        m_attackTween = DOVirtual
            .DelayedCall(m_fighter.settings.bulletSalvoRateOfFire, m_fighter.Fire)
            .SetLoops(m_fighter.settings.bulletSalvoCount)
            .OnComplete(AttackComplete)
            .SetDelay(m_fighter.settings.delayBetweenSalvo);
    }

    private void AttackComplete()
    {
        ChangeState((int)EnemyStateType.Move);
    }
}
