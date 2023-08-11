using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    public class HarvesterMoveState : EnemyMoveState
    {
        public HarvesterMoveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
        }

        private Harvester m_harvester;

        private Tween m_approachCrystal;

        protected override void DefaultEnter()
        {
            base.DefaultEnter();

            Vector3 direction =
                m_harvester.targetCrystal.transform.position - m_harvester.transform.position;
            float distance = direction.magnitude;
            float duration = distance / m_harvester.settings.speed;

            Vector3 endPosition =
                m_harvester.targetCrystal.transform.position +
                (
                    m_harvester.settings.offsetFromShard *
                    m_harvester.targetCrystal.transform.localScale.x *
                    -direction.normalized
                );
            m_harvester.transform.up = direction.normalized;
            m_approachCrystal = m_harvester.transform.DOMove(endPosition, duration).OnComplete(OnCompleteMovement);
        }

        public override void Update()
        {
            if (!m_harvester.isCrystalValid)
            {
                Debug.Log("target crystal is not valid !!!!!");
                ChangeState((int)EnemyStateType.Idle);
            }
        }

        public override void Exit()
        {
            if (m_approachCrystal != null && m_approachCrystal.IsPlaying())
                m_approachCrystal.Kill();
        }

        public override void Pause()
        {
            if (m_approachCrystal != null && m_approachCrystal.IsPlaying())
                m_approachCrystal.Pause();
        }

        public override void Resume()
        {
            if (m_approachCrystal != null && !m_approachCrystal.IsPlaying())
                m_approachCrystal.Play();
        }

        public void OnCompleteMovement()
        {
            ChangeState((int)EnemyStateType.Attack);
        }

    }
}