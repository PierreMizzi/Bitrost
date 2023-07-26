using DG.Tweening;
using UnityEngine;

public class EnemyIdleState : AState
{
    public EnemyIdleState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        type = (int)EnemyStateType.Idle;
    }

    protected override void DefaultEnter()
    {
        base.DefaultEnter();
        float delay = Random.Range(2f, 5f);
        DOVirtual.DelayedCall(
            delay,
            () =>
            {
                m_stateMachine.ChangeState(type, (int)EnemyStateType.Move);
            }
        );
    }
}
