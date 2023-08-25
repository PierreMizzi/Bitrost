using System;
using System.Collections.Generic;
using Bitfrost.Gameplay.Energy;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    [RequireComponent(typeof(Animator))]
    public class Harvester : Enemy
    {

        #region Fields 

        public new HarvesterSettings settings
        {
            get { return base.settings as HarvesterSettings; }
        }

        public Predicate<CrystalShard> m_targetableCrystalPredicate;

        public CrystalShard targetCrystal { get; private set; }

        public bool isTargetValid
        {
            get
            {
                return targetCrystal != null && targetCrystal.hasEnergy;
            }
        }

        public CircularSpacerSpot targetSpot { get; private set; }

        #endregion

        #region Methods 

        #endregion

        #region Behaviour

        protected override void Awake()
        {
            base.Awake();

            m_targetableCrystalPredicate = (CrystalShard crystal) => crystal.isTargetableByHarvesters;

            m_hitSounds = new List<string>(){
                SoundDataID.HARVESTER_HIT_01,
                SoundDataID.HARVESTER_HIT_02,
            };
        }

        public override void InitiliazeStates()
        {
            states = new List<AState>()
            {
                new EnemyInactiveState(this),
                new HarvesterIdleState(this),
                new HarvesterMoveState(this),
                new HarvesterAttackState(this),
                new EnemyDeadState(this),
            };
        }

        protected override void CallbackNoHealth()
        {
            targetCrystal = null;
            targetSpot.isAvailable = true;
            targetSpot = null;
            base.CallbackNoHealth();
        }

        #endregion

        #region Crystal Shards

        public void SearchTargetCrystal()
        {
            // TODO : Same as Blockers
            // First, target all currently used crystals 
            CrystalShard crystal = PickRandomTargetableCrystal(
                m_levelChannel.crystalManager.occupiedCrystals
            );
            if (crystal != null)
                goto Found;

            // If none, picks from crystals at player's range
            crystal = PickRandomTargetableCrystal(CrystalsAroundPlayer());
            if (crystal != null)
                goto Found;

            // If none, picks from all crystals
            crystal = PickRandomTargetableCrystal(m_levelChannel.crystalManager.activeCrystals);
            if (crystal != null)
                goto Found;

            Found:
            {
                targetCrystal = crystal;
            }
        }

        public void SearchTargetSpot()
        {
            if (targetCrystal == null)
                return;

            targetSpot = targetCrystal.harvesterCircularSpacer.GetSpotReworked(transform.position);
        }

        [Obsolete]
        private CrystalShard PickRandomTargetableCrystal(List<CrystalShard> crystals)
        {
            List<CrystalShard> targetableCrystals = crystals.FindAll(
                (CrystalShard crystal) => crystal.isTargetableByHarvesters
            );

            if (targetableCrystals.Count == 0)
                return null;
            else
            {
                int index = UnityEngine.Random.Range(0, targetableCrystals.Count - 1);
                return targetableCrystals[index];
            }
        }

        [Obsolete]
        private List<CrystalShard> CrystalsAroundPlayer()
        {
            List<CrystalShard> crystals = new List<CrystalShard>();

            Vector3 playerPosition = levelChannel.player.transform.position;
            float sqrDistance;

            foreach (CrystalShard crystal in levelChannel.crystalManager.activeCrystals)
            {
                sqrDistance = (playerPosition - crystal.transform.position).sqrMagnitude;

                if (sqrDistance < settings.rangeAroundPlayerSqr)
                    crystals.Add(crystal);
            }

            return crystals;
        }

        #endregion

        #region Pause

        public override void Pause()
        {
            base.Pause();
            animator.speed = 0;
        }

        public override void Resume()
        {
            base.Resume();
            animator.speed = 1;
        }

        #endregion
    }
}
