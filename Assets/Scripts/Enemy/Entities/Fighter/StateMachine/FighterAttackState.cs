using System.Collections;
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

    private IEnumerator m_attackCoroutine = null;

    protected override void DefaultEnter()
    {
        base.DefaultEnter();

        StartAttack();
    }

    public override void Update()
    {
        m_fighter.transform.up = m_fighter.directionTowardPlayer;
    }

    private void AttackComplete()
    {
        m_fighter.StopCoroutine(m_attackCoroutine);
        m_attackCoroutine = null;

        ChangeState((int)EnemyStateType.Move);
    }

    private void StartAttack()
    {
        m_attackCoroutine = AttackCoroutine();
        m_fighter.StartCoroutine(m_attackCoroutine);
    }

    private IEnumerator AttackCoroutine()
    {
        
        for (int i = 0; i < m_fighter.settings.bulletSalvoCount; i++)
        {
            m_fighter.Fire();
            yield return new WaitForSeconds(m_fighter.settings.bulletSalvoRateOfFire);
        }

        yield return new WaitForSeconds(m_fighter.settings.delayBetweenSalvo);
        AttackComplete();
    }
}
