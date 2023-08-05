using System.Collections.Generic;
using CodesmithWorkshop.Useful;
using UnityEngine;

/*

    // TODO : Refacto Bullet behaviour

    Scout

    Appear -> Find a positions around the Player
    Move -> Reach the position around the player
    Attack -> Track this position and occasionaly fire at the player

    Bullet -> Slow and 15 dmg


*/

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

    public float speedTrackPlayer { get; private set; }

    #endregion

    #region Methods 

    public override void OutOfPool(EnemyManager manager)
    {
        base.OutOfPool(manager);
        m_randomDirectionAroundPlayer = CloseRandomDirectionFromPlayer(settings.angleAroundPlayer);
        speedTrackPlayer = Random.Range(settings.minSpeedTrackPlayer, settings.maxSpeedTrackPlayer);
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
        };

    }

    protected void LateUpdate()
    {
        if (!isPaused)
            currentState?.Update();
    }

    public void Fire()
    {
        if (CanFire())
        {
            bulletChannel.onInstantiateBullet.Invoke(this, settings.bulletPrefab, transform.position, transform.up);
        }
    }

    public bool CanFire()
    {
        return true;
    }

    #endregion

}
