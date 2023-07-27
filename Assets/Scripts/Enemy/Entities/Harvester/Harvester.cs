using System.Collections.Generic;
using CodesmithWorkshop.Useful;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Harvester : Enemy
{
    // [Header("Header")]
    public HarvesterSettings settings
    {
        get { return base.settings as HarvesterSettings; }
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
        CrystalShard crystal = PickRandomValidCrystal(
            m_levelChannel.crystalManager.unavailableCrystals
        );
        if (crystal != null)
            goto Found;

        // If none, picks from crystals at player's range
        crystal = PickRandomValidCrystal(CrystalsAroundPlayer());
        if (crystal != null)
            goto Found;

        // If none, picks from all crystals
        crystal = PickRandomValidCrystal(m_levelChannel.crystalManager.crystals);
        if (crystal != null)
            goto Found;

        Found:
        {
            targetCrystal = crystal;
        }
    }

    private CrystalShard PickRandomValidCrystal(List<CrystalShard> crystals)
    {
        List<CrystalShard> validCrystals = crystals.FindAll(
            (CrystalShard crystal) => crystal.hasEnergy
        );

        if (validCrystals.Count == 0)
            return null;
        else
        {
            int index = Random.Range(0, validCrystals.Count - 1);
            return validCrystals[index];
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

    protected override void CallbackNoHealth()
    {
        targetCrystal = null;
        ChangeState((EnemyStateType)currentState.type, EnemyStateType.Inactive);
        base.CallbackNoHealth();
    }
}
