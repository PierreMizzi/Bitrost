using DG.Tweening;
using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Enemies
{

    /// <summary>
    /// Harvesters attacks crystals shard 
    /// </summary>
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

        #region AState

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            Attack();
        }

        public override void Update()
        {
            m_harvester.transform.position = m_harvester.targetSpot.transform.position;
            m_harvester.transform.up = m_harvester.targetSpot.transform.up;

            if (!m_harvester.isTargetValid)
                ChangeState((int)EnemyStateType.Idle);
        }

        public override void Exit()
        {
            base.Exit();
            m_harvester.animator.SetBool(IS_ATTACKING_BOOL, false);
            m_attackSequence.Kill();
        }

        public override void Pause()
        {
            base.Pause();

            if (m_attackSequence != null && m_attackSequence.IsPlaying())
                m_attackSequence.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (m_attackSequence != null && !m_attackSequence.IsPlaying())
                m_attackSequence.Play();
        }

        #endregion

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
