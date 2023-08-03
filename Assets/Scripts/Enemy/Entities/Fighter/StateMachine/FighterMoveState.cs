using DG.Tweening;
using UnityEngine;

public class FighterMoveState : EnemyMoveState
{
    public FighterMoveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_fighter = m_stateMachine.gameObject.GetComponent<Fighter>();
    }

    private Fighter m_fighter = null;

    private Vector3 m_endPosition;

    private Tween m_tween;

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
        m_tween.Kill();
    }

    private void ApproachPlayer()
    {
        Vector3 playerPosition = m_fighter.levelChannel.player.transform.position;
        m_endPosition = m_fighter.CloseRandomPositionAroundPlayer(m_fighter.settings.radiusAroundPlayer, m_fighter.settings.angleAroundPlayer);

        // Duration of movement
        float duration = (playerPosition - m_endPosition).magnitude / m_fighter.settings.speed;

        m_tween = m_fighter.transform
            .DOMove(m_endPosition, duration)
            .OnComplete(ApproachPlayerComplete);
    }

    private void ApproachPlayerComplete()
    {
        ChangeState((int)EnemyStateType.Attack);
    }
}
