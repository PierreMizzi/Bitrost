using System.Collections;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{
    /// <summary>
    /// After reaching it's position around the player, the FIghter fires a series of bullet before finding another position
    /// </summary>
    public class FighterAttackState : EnemyAttackState
    {
        public FighterAttackState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
        }

        private Fighter m_fighter = null;

        private IEnumerator m_attackCoroutine = null;

        #region AState

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
            base.Pause();

            PauseAttack();
        }

        public override void Resume()
        {
            base.Resume();

            ResumeMethod();
        }

        #endregion


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

        private void PauseAttack()
        {
            if (m_attackCoroutine != null)
                m_fighter.StopCoroutine(m_attackCoroutine);
        }

        private void ResumeMethod()
        {
            if (m_attackCoroutine != null)
                m_fighter.StartCoroutine(m_attackCoroutine);
        }

        private IEnumerator AttackCoroutine()
        {
            // Salvo of bullet fired at the player
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
