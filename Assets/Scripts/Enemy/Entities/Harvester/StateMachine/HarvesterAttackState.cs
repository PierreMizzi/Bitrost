using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{
    public class HarvesterAttackState : EnemyAttackState
    {
        public HarvesterAttackState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
        }

        private Harvester m_harvester = null;

        private Sequence m_attackSequence = null;

        private const string IS_ATTACKING_BOOL = "IsAttacking";

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            Attack();
        }

        public override void Update()
        {
            if (!m_harvester.isCrystalValid)
            {
                Debug.Log("target crystal is not valid !!!!! but in ATTACK");
                ChangeState((int)EnemyStateType.Idle);
            }
        }

        public override void Exit()
        {
            base.Exit();
            m_harvester.animator.SetBool(IS_ATTACKING_BOOL, false);
            m_attackSequence.Kill();
        }

        public override void Pause()
        {
            if (m_attackSequence != null && m_attackSequence.IsPlaying())
                m_attackSequence.Pause();
        }

        public override void Resume()
        {
            if (m_attackSequence != null && !m_attackSequence.IsPlaying())
                m_attackSequence.Play();
        }

        public void Attack()
        {
            m_attackSequence = DOTween.Sequence();
            m_attackSequence
                .AppendInterval(m_harvester.settings.attackDelay)
                .AppendCallback(StartAttack)
                .AppendInterval(m_harvester.settings.attackSpeed)
                .AppendCallback(CompleteAttack);
        }

        public void StartAttack()
        {
            m_harvester.animator.SetBool(IS_ATTACKING_BOOL, true);
        }

        public void CompleteAttack()
        {
            m_harvester.animator.SetBool(IS_ATTACKING_BOOL, false);
            m_harvester.targetCrystal.DecrementEnergy();

            if (m_harvester.targetCrystal.hasEnergy)
                Attack();
            else
                ChangeState((int)EnemyStateType.Idle);
        }

    }
}
