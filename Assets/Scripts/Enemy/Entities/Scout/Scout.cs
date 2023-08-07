using System.Collections.Generic;
using PierreMizzi.SoundManager;
using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();

        m_hitSounds = new List<string>(){
            SoundDataIDStatic.SCOUT_HIT_01,
            SoundDataIDStatic.SCOUT_HIT_02,
        };
    }

    protected void LateUpdate()
    {
        if (!isPaused)
            currentState?.Update();
    }

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

    public void Fire()
    {
        if (CanFire())
        {
            bulletChannel.onInstantiateBullet.Invoke(this, settings.bulletPrefab, transform.position, transform.up);
            SoundManager.PlaySound(SoundDataIDStatic.SCOUT_BULLET);
        }
    }

    public bool CanFire()
    {
        return true;
    }

    #endregion

}
