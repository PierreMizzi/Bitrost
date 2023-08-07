using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{
    public class FighterMoveState : EnemyMoveState
    {
        public FighterMoveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
        }

        private Fighter m_fighter;

        private Vector3 m_endPosition;

        private Tween m_approachPlayerTween;

        protected override void DefaultEnter()
        {
            ApproachPlayer();
        }

        public override void Update()
        {
            m_fighter.transform.up = m_fighter.directionTowardPlayer;
        }

        public override void Exit()
        {
            if (m_approachPlayerTween != null && m_approachPlayerTween.IsPlaying())
                m_approachPlayerTween.Kill();
        }

        public override void Pause()
        {
            if (m_approachPlayerTween != null && m_approachPlayerTween.IsPlaying())
                m_approachPlayerTween.Pause();
        }

        public override void Resume()
        {
            if (m_approachPlayerTween != null && !m_approachPlayerTween.IsPlaying())
                m_approachPlayerTween.Play();
        }

        private void ApproachPlayer()
        {
            Vector3 playerPosition = m_fighter.levelChannel.player.transform.position;
            m_endPosition = m_fighter.CloseRandomPositionAroundPlayer(m_fighter.settings.radiusAroundPlayer, m_fighter.settings.angleAroundPlayer);

            // Duration of movement
            float duration = (playerPosition - m_endPosition).magnitude / m_fighter.settings.speed;

            m_approachPlayerTween = m_fighter.transform
                .DOMove(m_endPosition, duration)
                .OnComplete(ApproachPlayerComplete);
        }

        private void ApproachPlayerComplete()
        {
            ChangeState((int)EnemyStateType.Attack);
        }

    }
}
