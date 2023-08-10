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

        public CrystalShard targetCrystal { get; private set; }

        public bool isCrystalValid
        {
            get
            {
                return targetCrystal != null && targetCrystal.hasEnergy;
            }
        }

        public Animator animator { get; private set; }

        #endregion

        #region Methods 

        #endregion

        #region Behaviour

        protected override void Awake()
        {
            base.Awake();

            m_hitSounds = new List<string>(){
                SoundDataID.HARVESTER_HIT_01,
                SoundDataID.HARVESTER_HIT_02,
            };
        }

        protected override void Initialize()
        {
            animator = GetComponent<Animator>();
            base.Initialize();
        }

        public override void InitiliazeStates()
        {
            states = new List<AState>()
            {
                new EnemyInactiveState(this),
                new HarvesterIdleState(this),
                new HarvesterMoveState(this),
                new HarvesterAttackState(this),
            };
        }

        protected override void CallbackNoHealth()
        {
            targetCrystal = null;
            base.CallbackNoHealth();
        }

        #endregion

        #region Crystal Shards

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
            crystal = PickRandomValidCrystal(m_levelChannel.crystalManager.activeCrystals);
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
