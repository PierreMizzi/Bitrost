using System.Collections;
using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    public class FighterAttackState : EnemyAttackState
    {
        public FighterAttackState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
        }

        private Fighter m_fighter = null;

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

        public override void Exit()
        {
            base.Exit();
            StopAttack();
        }

        public override void Pause()
        {
            if (m_attackCoroutine != null)
                m_fighter.StopCoroutine(m_attackCoroutine);
        }

        public override void Resume()
        {
            if (m_attackCoroutine != null)
                m_fighter.StartCoroutine(m_attackCoroutine);
        }

        private void StartAttack()
        {
            m_attackCoroutine = AttackCoroutine();
            m_fighter.StartCoroutine(m_attackCoroutine);
        }

        private void StopAttack()
        {
            m_fighter.StopCoroutine(m_attackCoroutine);
            m_attackCoroutine = null;
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

        private void AttackComplete()
        {
            ChangeState((int)EnemyStateType.Move);
        }

    }
}
