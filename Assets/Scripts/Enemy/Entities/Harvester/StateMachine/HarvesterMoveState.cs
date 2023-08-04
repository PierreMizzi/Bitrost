using DG.Tweening;
using UnityEngine;

public class HarvesterMoveState : EnemyMoveState
{
    public HarvesterMoveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
    }

    private Harvester m_harvester = null;

    private Tween m_approachCrystal;

    protected override void DefaultEnter()
    {
        base.DefaultEnter();
        m_harvester.SearchCrystalShard();

        Vector3 direction =
            m_harvester.targetCrystal.transform.position - m_harvester.transform.position;
        float distance = direction.magnitude;
        float duration = distance / m_harvester.settings.speed;

        Vector3 endPosition =
            m_harvester.targetCrystal.transform.position
            + (
                -direction.normalized
                * m_harvester.targetCrystal.transform.localScale.x
                * m_harvester.settings.offsetFromShard
            );
        m_harvester.transform.up = direction.normalized;
        m_approachCrystal = m_harvester.transform.DOMove(endPosition, duration).OnComplete(OnCompleteMovement);
    }

    public override void Exit()
    {
        base.Exit();
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
