using System.Collections.Generic;
using CodesmithWorkshop.Useful;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Harvester : Enemy
{
    [SerializeField]
    private LevelChannel m_levelChannel = null;

    public LevelChannel levelChannel
    {
        get { return m_levelChannel; }
    }

    public HarvesterSettings settings
    {
        get { return m_settings as HarvesterSettings; }
    }

    public CrystalShard targetCrystal { get; private set; }

    public Animator animator { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        m_healthEntity = GetComponent<HealthEntity>();
    }

    public override void InitiliazeState()
    {
        states = new List<AState>()
        {
            new EnemyInactiveState(this),
            new EnemyIdleState(this),
            new HarvesterMoveState(this),
            new HarvesterAttackState(this),
        };
    }

    public void SearchCrystalShard()
    {
        // First, targe tall currently used crystals
        CrystalShard crystal = UtilsClass.PickRandomInList(
            m_levelChannel.crystalManager.unavailableCrystals
        );
        if (crystal != null)
            goto Found;

        // If none, picks from crystals at player's range
        crystal = UtilsClass.PickRandomInList(CrystalsAroundPlayer());
        if (crystal != null)
            goto Found;

        // If none, picks from all crystals
        crystal = UtilsClass.PickRandomInList(m_levelChannel.crystalManager.crystals);
        if (crystal != null)
            goto Found;

        Found:
        {
            targetCrystal = crystal;
        }
    }

    private List<CrystalShard> CrystalsAroundPlayer()
    {
        List<CrystalShard> crystals = new List<CrystalShard>();

        Vector3 playerPosition = levelChannel.player.transform.position;
        float sqrDistance;

        foreach (CrystalShard crystal in levelChannel.crystalManager.crystals)
        {
            sqrDistance = (playerPosition - crystal.transform.position).sqrMagnitude;

            if (sqrDistance < settings.rangeAroundPlayerSqr)
                crystals.Add(crystal);
        }

        return crystals;
    }

    public override void Initialize(EnemyManager manager)
    {
        base.Initialize(manager);
        ChangeState((EnemyStateType)currentState.type, EnemyStateType.Idle);
    }

    protected override void CallbackNoHealth()
    {
        targetCrystal = null;
        ChangeState((EnemyStateType)currentState.type, EnemyStateType.Inactive);
        base.CallbackNoHealth();
    }
}
