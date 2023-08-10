using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;

namespace Bitfrost.Gameplay.Turrets
{

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
            m_turret.aimSprite.SetActive(true);

            SoundManager.PlaySFX(SoundDataID.TURRET_FIRE_MODE);
        }

        public override void Update()
        {
            base.Update();
            m_turret.ComputeAimDirection();
            m_turret.canonTransform.up = m_turret.aimDirection;

            if (!m_turret.hasEnergy)
                ChangeState(TurretStateType.Disabled);

        }

    }
}
