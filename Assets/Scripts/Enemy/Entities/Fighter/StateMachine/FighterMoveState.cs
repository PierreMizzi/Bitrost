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
        Vector3 playerToFighter = (m_fighter.transform.position - playerPosition);

        // Set Position
        float randomAngle = Random.Range(
            -m_fighter.settings.angleAroundPlayer,
            m_fighter.settings.angleAroundPlayer
        );
        playerToFighter = Quaternion.AngleAxis(randomAngle, Vector3.forward) * playerToFighter;

        m_endPosition =
            playerPosition + (playerToFighter.normalized * m_fighter.settings.radiusAroundPlayer);

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
