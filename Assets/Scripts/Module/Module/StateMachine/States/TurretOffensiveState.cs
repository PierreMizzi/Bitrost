using UnityEngine;

public class TurretOffensiveState : ATurretState
{
    public TurretOffensiveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        type = (int)TurretStateType.Offensive;
    }


    protected override void DefaultEnter()
    {
        base.DefaultEnter();

        m_turret.SetActive();
        m_turret.canonTransform.gameObject.SetActive(true);
    }

    public override void Update()
    {
        base.Update();
        m_turret.ComputeAimDirection();
        m_turret.canonTransform.up = m_turret.aimDirection;
    }

}
