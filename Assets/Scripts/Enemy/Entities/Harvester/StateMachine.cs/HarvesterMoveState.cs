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

    protected override void DefaultEnter()
    {
        base.DefaultEnter();
        m_harvester.SearchCrystalShard();

        Debug.Log($"targetCrystal : {m_harvester.targetCrystal.name}");

        Vector3 direction =
            m_harvester.targetCrystal.transform.position - m_harvester.transform.position;
        float distance = direction.magnitude;
        float duration = distance / m_harvester.speed;

        Vector3 endPosition =
            m_harvester.targetCrystal.transform.position
            + (
                -direction.normalized
                * m_harvester.targetCrystal.transform.localScale.x
                * m_harvester.offsetFromShard
            );
        m_harvester.transform.up = direction.normalized;
        m_harvester.transform.DOMove(endPosition, duration).OnComplete(OnCompleteMovement);
    }

    public void OnCompleteMovement()
    {
        ChangeState((int)EnemyStateType.Attack);
    }
}
