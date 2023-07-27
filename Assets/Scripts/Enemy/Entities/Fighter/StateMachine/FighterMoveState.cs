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

    private void ApproachPlayer()
    {
        Vector3 playerPosition = m_fighter.levelChannel.player.transform.position;
        Vector3 playerToFighter = (m_fighter.transform.position - playerPosition);
        m_endPosition = playerToFighter.normalized * m_fighter.settings.radiusAroundPlayer;

        float duration = playerToFighter.magnitude / m_fighter.settings.speed;

        m_fighter.transform.up = -playerToFighter.normalized;
        m_tween = m_fighter.transform.DOMove(m_endPosition, duration).OnComplete(ApproachPlayerComplete);
    }

    private void ApproachPlayerComplete()
    {
        ChangeState((int)EnemyStateType.Attack);
    }
}
