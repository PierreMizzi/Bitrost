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

            m_this.SetActive();
            m_this.canonTransform.gameObject.SetActive(true);
            m_this.aimSprite.SetActive(true);

            SoundManager.PlaySFX(SoundDataID.TURRET_FIRE_MODE);
        }

        public override void Update()
        {
            base.Update();
            m_this.ComputeAimDirection();
            m_this.canonTransform.up = m_this.aimDirection;

            if (!m_this.hasEnergy)
                ChangeState(TurretStateType.Disabled);

        }

    }
}
