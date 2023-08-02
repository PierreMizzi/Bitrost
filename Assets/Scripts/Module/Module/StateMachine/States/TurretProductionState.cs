using DG.Tweening;

public class TurretProductionState : ATurretState
{
    public TurretProductionState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        type = (int)TurretStateType.Production;
    }

    private float m_productionProgress = 0;

    private Tween m_productionCycleTween;

    protected override void DefaultEnter()
    {
        base.DefaultEnter();

        m_turret.canonTransform.gameObject.SetActive(false);
        StartProduction();
    }

    public override void Exit()
    {
        m_productionProgress = 0;
        m_turret.onUpdateExtractionUI.Invoke(m_productionProgress);

        if (m_productionCycleTween != null && m_productionCycleTween.IsPlaying())
            m_productionCycleTween.Kill();
    }

    private void StartProduction()
    {
        m_productionProgress = 0;
        m_productionCycleTween = DOVirtual
            .Float(0, 1, m_turret.settings.productionDuration, ProductionUpdate)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .OnStepComplete(CompleteProductionCycle);

        m_turret.onExtraction.Invoke();
    }

    private void ProductionUpdate(float value)
    {
        m_productionProgress = value;
        m_turret.onUpdateExtractionUI.Invoke(m_productionProgress);
    }

    public void CompleteProductionCycle()
    {
        m_turret.crystal.DecrementEnergy();

        m_turret.storedEnergy += m_turret.settings.productionRatio;

        if (!m_turret.CanBeProduction())
            ChangeState((int)TurretStateType.Offensive);

        m_productionProgress = 0;
        m_turret.onUpdateExtractionUI.Invoke(m_productionProgress);
        m_turret.onRefreshEnergy.Invoke();
    }
}
