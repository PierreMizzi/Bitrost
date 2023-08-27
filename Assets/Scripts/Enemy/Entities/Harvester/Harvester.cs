using System;
using System.Collections.Generic;
using Bitfrost.Gameplay.Energy;
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

        public Spot targetSpot { get; private set; }

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
            // First, target all currently used crystals 
            CrystalShard crystal = m_levelChannel.crystalManager.PickRandomOccupiedCrystal(m_targetableCrystalPredicate);
            if (crystal != null)
                goto Found;

            // If none, picks from crystals at player's range
            crystal = m_levelChannel.crystalManager.PickRandomCrystalNearPlayer(settings.searchCrystalRangeSqr, m_targetableCrystalPredicate);
            if (crystal != null)
                goto Found;

            // If none, picks from all crystals
            crystal = m_levelChannel.crystalManager.PickRandomActiveCrystal(m_targetableCrystalPredicate);
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

            targetSpot = targetCrystal.harvesterSpotManager.GetSpot(transform.position);
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
