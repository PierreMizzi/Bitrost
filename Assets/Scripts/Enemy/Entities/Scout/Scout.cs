using System.Collections.Generic;
using Bitfrost.Gameplay.Bullets;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    /// <summary>
    /// Scout are weak enemies that follows the player and fires a single bullet at him 
    /// </summary>
    public class Scout : Enemy, IBulletLauncher
    {

        #region Fields 


        public new ScoutSettings settings
        {
            get { return base.settings as ScoutSettings; }
        }

        [SerializeField]
        private BulletChannel m_bulletChannel = null;

        public BulletChannel bulletChannel { get { return m_bulletChannel; } }

        public Vector3 m_randomDirectionAroundPlayer;

        public Vector3 positionAroundPlayer { get { return m_levelChannel.player.transform.position + (m_randomDirectionAroundPlayer * settings.radiusAroundPlayer); } }


        public float speedTrackPlayer;

        #endregion

        #region Methods 

        protected override void Awake()
        {
            base.Awake();

            m_hitSounds = new List<string>(){
            SoundDataID.SCOUT_HIT_01,
            SoundDataID.SCOUT_HIT_02,
        };
        }

        protected void LateUpdate()
        {
            if (!isPaused)
                currentState?.Update();
        }

        public override void OutOfPool(EnemyManager manager)
        {
            m_randomDirectionAroundPlayer = RandomDirectionAroundPlayer(settings.angleAroundPlayer);
            speedTrackPlayer = Random.Range(settings.minSpeedTrackPlayer, settings.maxSpeedTrackPlayer);
            base.OutOfPool(manager);
        }

        public override void InitiliazeStates()
        {
            base.InitiliazeStates();

            states = new List<AState>()
            {
                new EnemyInactiveState(this),
                new EnemyIdleState(this),
                new ScoutMoveState(this),
                new ScoutAttackState(this),
                new EnemyDeadState(this),
            };
        }

        public void Fire()
        {
            if (CanFire())
            {
                bulletChannel.onFireBullet.Invoke(settings.bulletConfig, this, transform.position, transform.up);
                SoundManager.PlaySFX(SoundDataID.SCOUT_BULLET);
            }
        }

        public bool CanFire()
        {
            return true;
        }

        #endregion

    }
}
