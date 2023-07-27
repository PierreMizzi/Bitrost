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
        float delay = Random.Range(1f, 2f);
        DOVirtual.DelayedCall(
            delay,
            () =>
            {
                ChangeState((int)EnemyStateType.Move);
            }
        );
    }
}
