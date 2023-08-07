using System.Collections.Generic;
using PierreMizzi.SoundManager;
using UnityEngine;

public partial class Module : MonoBehaviour, IStateMachine
{
    #region Fields

    public AState currentState { get; set; }
    public List<AState> states { get; set; }

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
            SetProductionMode();
        else
            SoundManager.PlaySound(SoundDataIDStatic.TURRET_WRONG_ACTION);
    }

    private void ProductionToOffensive()
    {
        if (CanBeOffensive())
            SetOffensiveMode();
        else
            SoundManager.PlaySound(SoundDataIDStatic.TURRET_WRONG_ACTION);
    }

    public void SetOffensiveMode()
    {
        ChangeState(TurretStateType.Offensive, TurretStateType.Production);

    }

    private void SetProductionMode()
    {
        ChangeState(TurretStateType.Production, TurretStateType.Offensive);
    }

    public bool CanBeOffensive()
    {
        return true;
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

    #endregion
}
