using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{
    /// <summary>
    /// Finds a position around the player and starts attacking after reaching it
    /// </summary>
    public class FighterMoveState : EnemyMoveState
    {
        public FighterMoveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
        }

        private Fighter m_fighter;

        private Tween m_approachPlayerTween;

        #region AState

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
            base.Pause();

            if (m_approachPlayerTween != null && m_approachPlayerTween.IsPlaying())
                m_approachPlayerTween.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (m_approachPlayerTween != null && !m_approachPlayerTween.IsPlaying())
                m_approachPlayerTween.Play();
        }

        #endregion

        private void ApproachPlayer()
        {
            Vector3 playerPosition = m_fighter.levelChannel.player.transform.position;

            // position around the player
            Vector3 endPosition = m_fighter.RandomPositionAroundPlayer(m_fighter.settings.radiusAroundPlayer, m_fighter.settings.angleAroundPlayer);

            // Duration of movement
            float duration = (playerPosition - endPosition).magnitude / m_fighter.settings.speed;

            m_approachPlayerTween = m_fighter.transform
                .DOMove(endPosition, duration)
                .OnComplete(ApproachPlayerComplete);
        }

        private void ApproachPlayerComplete()
        {
            ChangeState((int)EnemyStateType.Attack);
        }

    }
}
