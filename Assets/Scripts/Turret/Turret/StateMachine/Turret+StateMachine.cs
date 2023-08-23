using System.Collections.Generic;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{

    public partial class Turret : MonoBehaviour, IStateMachine
    {
        #region Fields

        public AState currentState { get; set; }

        public TurretStateType currentStateType { get { return (TurretStateType)currentState.type; } }

        public List<AState> states { get; set; }

        public Animator m_animator;

        #endregion

        #region Methods

        #region State Machine

        public void InitiliazeStates()
        {
            states = new List<AState>()
        {
            new TurretInactiveState(this),
            new TurretOffensiveState(this),
            new TurretProductionState(this),
            new TurretDisabledState(this),
        };
        }

        public void ChangeState(
            TurretStateType nextState,
            TurretStateType previousState = TurretStateType.None
        )
        {
            ChangeState((int)previousState, (int)nextState);
        }

        public void ChangeState(int previousState, int nextState)
        {
            currentState?.Exit();

            currentState = states.Find((AState newState) => newState.type == nextState);
            if (currentState != null)
            {
                currentState.Enter(previousState);
                onChangeState((TurretStateType)nextState);
            }
            else
            {
                Debug.LogError($"Couldn't find a new state of type : {nextState}. Going Inactive");
            }
        }

        public void UpdateState()
        {
            if (!isPaused)
                currentState?.Update();
        }

        #endregion

        public void SwitchMode()
        {
            TurretStateType currentStateType = (TurretStateType)currentState.type;
            switch (currentStateType)
            {
                case TurretStateType.Offensive:
                    OffensiveToProduction();
                    break;

                case TurretStateType.Production:
                    ProductionToOffensive();
                    break;
            }
        }

        private void OffensiveToProduction()
        {
            if (CanBeProduction())
                ChangeState(TurretStateType.Production, TurretStateType.Offensive);
            else
                SoundManager.PlaySFX(SoundDataID.TURRET_WRONG_ACTION);
        }

        private void ProductionToOffensive()
        {
            ChangeState(TurretStateType.Offensive, TurretStateType.Production);
        }

        public bool CanBeProduction()
        {
            bool result = true;

            result &= canStoreEnergy;
            if (!canStoreEnergy)
                Debug.LogWarning("CAN'T STORE ENERGY");

            // Crystal has energy ?
            result &= crystal.hasEnergy;
            if (!crystal.hasEnergy)
                Debug.LogWarning("CRYSTAL IS DEPLEATED");

            return result;
        }

        private const string k_turretStateType = "TurretStateType";

        public void ChangeAnimatorState(int state)
        {
            m_animator.SetInteger(k_turretStateType, state);
        }

        #endregion
    }
}
