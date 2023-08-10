using System.Collections.Generic;
using Bitfrost.Gameplay.Bullets;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{
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
            SoundDataIDStatic.FIGHTER_HIT_01,
            SoundDataIDStatic.FIGHTER_HIT_02,
        };
        }

        public override void InitiliazeStates()
        {
            states = new List<AState>()
        {
            new EnemyInactiveState(this),
            new EnemyIdleState(this),
            new FighterMoveState(this),
            new FighterAttackState(this)
        };
        }

        #endregion


        #region IBulletLauncher

        public bool CanFire()
        {
            return true;
        }

        public void Fire()
        {
            if (CanFire())
            {
                Vector3 playerPosition = levelChannel.player.transform.position;
                Vector3 fighterToPlayer = playerPosition - transform.position;

                bulletChannel.onInstantiateBullet(
                    this,
                    settings.bulletPrefab,
                    bulletOrigin.position,
                    fighterToPlayer.normalized
                );

                SoundManager.PlaySFX(SoundDataIDStatic.FIGHTER_BULLET);
            }
        }

        #endregion

        #endregion
    }
}
