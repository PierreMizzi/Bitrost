using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 directionTowardPlayer
    {
        get { return (m_levelChannel.player.transform.position - transform.position).normalized; }
    }

	#endregion

	#region Methods

	#region State Machine

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
        }
    }

    #endregion

	#endregion
}
