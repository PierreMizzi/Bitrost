using System.Collections.Generic;
using CodesmithWorkshop.Useful;
using UnityEngine;

public class Harvester : Enemy
{
    [SerializeField]
    private LevelChannel m_levelChannel = null;

    public LevelChannel levelChannel
    {
        get { return m_levelChannel; }
    }

    public CrystalShard targetCrystal { get; private set; }

    [SerializeField]
    private float m_rangeAroundPlayer = 10f;

    [HideInInspector]
    public float rangeAroundPlayerSqr;

    [SerializeField]
    private float m_speed = 80f;
    public float speed
    {
        get { return m_speed; }
    }

    [SerializeField]
    private float m_offsetFromShard = 0.4f;
    public float offsetFromShard
    {
        get { return m_offsetFromShard; }
    }

    [SerializeField]
    private float m_attackSpeed = 3f;
    public float attackSpeed
    {
        get { return m_attackSpeed; }
    }

    [SerializeField]
    private float m_attackDelay = 1f;
    public float attackDelay
    {
        get { return m_attackDelay; }
    }

    protected override void Awake()
    {
        base.Awake();

        rangeAroundPlayerSqr = Mathf.Pow(m_rangeAroundPlayer, 2f);
    }

    public override void InitiliazeState()
    {
        states = new List<AState>()
        {
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

            if (sqrDistance < rangeAroundPlayerSqr)
                crystals.Add(crystal);
        }

        return crystals;
    }
}
