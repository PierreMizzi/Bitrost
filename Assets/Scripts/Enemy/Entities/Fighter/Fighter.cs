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

	#endregion

	#region Methods

	#region State Machine

    public override void InitiliazeState()
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
        Vector3 playerPosition = levelChannel.player.transform.position;
        Vector3 fighterToPlayer = playerPosition - transform.position;

        bulletChannel.onInstantiateBullet(
            this,
            settings.bulletPrefab,
            bulletOrigin.position,
            fighterToPlayer.normalized
        );
    }

    #endregion

	#endregion
}
