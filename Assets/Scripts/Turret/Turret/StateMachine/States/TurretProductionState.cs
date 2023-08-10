using DG.Tweening;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Turrets
{

    public class TurretProductionState : ATurretState
    {
        public TurretProductionState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            type = (int)TurretStateType.Production;
        }

        private Tween m_productionCycleTween;

        protected override void DefaultEnter()
        {
            base.DefaultEnter();

            m_turret.canonTransform.gameObject.SetActive(false);
            StartProduction();

            SoundManager.PlaySFX(SoundDataID.TURRET_PRODUCTION_MODE);
            SoundManager.PlaySound(SoundDataID.TURRET_PRODUCTION_LOOP, true);
        }

        public override void Update()
        {
            if (!m_turret.crystal.hasEnergy)
            {
                if (m_turret.hasEnergy)
                    ChangeState(TurretStateType.Offensive);
                else
                    ChangeState(TurretStateType.Disabled);
            }
        }

        public override void Exit()
        {
            m_turret.productionProgress = 0;
            if (m_productionCycleTween != null && m_productionCycleTween.IsPlaying())
                m_productionCycleTween.Kill();

            SoundManager.StopSound(SoundDataID.TURRET_PRODUCTION_LOOP);
        }

        public override void Pause()
        {
            if (m_productionCycleTween != null && m_productionCycleTween.IsPlaying())
                m_productionCycleTween.Pause();
        }

        public override void Resume()
        {
            if (m_productionCycleTween != null && !m_productionCycleTween.IsPlaying())
                m_productionCycleTween.Play();
        }

        private void StartProduction()
        {
            m_turret.productionProgress = 0;
            m_productionCycleTween = DOVirtual
                .Float(0, 1, m_turret.settings.productionDuration, ProductionUpdate)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .OnStepComplete(CompleteProductionCycle);
        }

        private void ProductionUpdate(float value)
        {
            m_turret.productionProgress = value;
        }

        public void CompleteProductionCycle()
        {
            m_turret.crystal.DecrementEnergy();

            m_turret.storedEnergy += m_turret.settings.productionRatio;

            if (!m_turret.CanBeProduction())
                ChangeState(TurretStateType.Offensive);

            m_turret.productionProgress = 0;
            m_turret.onRefreshEnergy.Invoke();
        }
    }
}
