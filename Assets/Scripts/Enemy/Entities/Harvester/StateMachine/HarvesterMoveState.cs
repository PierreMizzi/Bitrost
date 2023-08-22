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

        private Sequence m_approachSpot;

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            m_harvester.SearchTargetSpot();
            m_harvester.targetSpot.isAvailable = false;

            Vector3 direction = m_harvester.targetSpot.position - m_harvester.transform.position;
            float distance = direction.magnitude;
            float durationMove = distance / m_harvester.settings.speed;

            m_harvester.transform.up = direction.normalized;

            m_approachSpot = DOTween.Sequence();
            m_approachSpot.Append(m_harvester.transform.DOMove(m_harvester.targetSpot.position, durationMove));
            m_approachSpot.Append(RotateTween());
            m_approachSpot.AppendCallback(OnCompleteMovement);
        }

        private Tween RotateTween()
        {
            Vector3 fromDirection = m_harvester.transform.up;
            Vector3 toDirection = -m_harvester.targetSpot.direction;
            float duration = 1f;

            return DOVirtual.Float(0, 1, duration, (float value) =>
            {
                m_harvester.transform.up = Vector3.Lerp(fromDirection, toDirection, value);
            });
        }

        public override void Update()
        {
            if (!m_harvester.isCrystalValid)
                ChangeState((int)EnemyStateType.Idle);
        }

        public override void Exit()
        {
            if (m_approachSpot != null && m_approachSpot.IsPlaying())
                m_approachSpot.Kill();
        }

        public override void Pause()
        {
            if (m_approachSpot != null && m_approachSpot.IsPlaying())
                m_approachSpot.Pause();
        }

        public override void Resume()
        {
            if (m_approachSpot != null && !m_approachSpot.IsPlaying())
                m_approachSpot.Play();
        }

        public void OnCompleteMovement()
        {
            ChangeState((int)EnemyStateType.Attack);
        }

    }
}