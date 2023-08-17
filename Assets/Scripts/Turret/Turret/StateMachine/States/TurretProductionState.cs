using DG.Tweening;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

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

        private const string k_progress = "_Progress";

        protected override void DefaultEnter()
        {
            base.DefaultEnter();

            m_this.ChangeAnimatorState(type);
            StartProduction();

            SoundManager.PlaySFX(SoundDataID.TURRET_PRODUCTION_MODE);
            SoundManager.PlaySound(SoundDataID.TURRET_PRODUCTION_LOOP, true);
        }

        public override void Update()
        {
            if (!m_this.crystal.hasEnergy)
            {
                if (m_this.hasEnergy)
                    ChangeState(TurretStateType.Offensive);
                else
                    ChangeState(TurretStateType.Disabled);
            }
        }

        public override void Exit()
        {
            m_this.productionProgress = 0;
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
            m_this.productionProgress = 0;
            m_productionCycleTween = DOVirtual
                .Float(0, 1, m_this.settings.productionDuration, ProductionUpdate)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .OnStepComplete(CompleteProductionCycle);
        }

        private void ProductionUpdate(float value)
        {
            m_this.productionProgress = value;

            m_this.factorySprite.transform.rotation = Quaternion.Euler(0, 0, -value * 360f);
            m_this.radialProgress.SetProperty(k_progress, value);
        }

        public void CompleteProductionCycle()
        {
            m_this.crystal.DecrementEnergy();

            m_this.storedEnergy += m_this.settings.productionRatio;

            if (!m_this.CanBeProduction())
                ChangeState(TurretStateType.Offensive);

            m_this.productionProgress = 0;
            m_this.onRefreshEnergy.Invoke(m_this.currentStateType);
        }
    }
}
