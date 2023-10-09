using System.Collections.Generic;
using Bitfrost.Gameplay.Bullets;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    /// <summary>
    /// Figther are tougher enemies that fires a series of bullet at the player
    /// </summary>
    public class Fighter : Enemy, IBulletLauncher
    {
        #region Fields

        [Header("Fighter")]
        public Transform bulletOrigin;

        public BulletChannel bulletChannel = null;

        public new FighterSettings settings
        {
            get { return base.settings as FighterSettings; }
        }

        #endregion

        #region Methods

        #region State Machine

        protected override void Awake()
        {
            base.Awake();

            m_hitSounds = new List<string>(){
            SoundDataID.FIGHTER_HIT_01,
            SoundDataID.FIGHTER_HIT_02,
        };
        }

        public override void InitiliazeStates()
        {
            states = new List<AState>()
            {
                new EnemyInactiveState(this),
                new EnemyIdleState(this),
                new EnemyDeadState(this),
                new FighterMoveState(this),
                new FighterAttackState(this)
            };
        }

        #endregion


        #region IBulletLauncher

        /// <summary>
        /// No seconds thought from a killing machine
        /// </summary>
        /// <returns>true</returns>
        public bool CanFire()
        {
            return true;
        }

        public void Fire()
        {
            if (CanFire())
            {
                Vector3 playerPosition = levelChannel.player.transform.position;
                Vector3 fighterToPlayer = (playerPosition - transform.position).normalized;

                bulletChannel.onFireBullet(
                    settings.bulletConfig,
                    this,
                    bulletOrigin.position,
                    fighterToPlayer
                );

                SoundManager.PlaySFX(SoundDataID.FIGHTER_BULLET);
            }
        }

        #endregion

        #endregion
    }
}
