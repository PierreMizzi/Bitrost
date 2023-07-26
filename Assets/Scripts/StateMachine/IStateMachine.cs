using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    public GameObject gameObject { get; }
    public AState currentState { get; set; }
    public List<AState> states { get; set; }
    public void InitiliazeState();
    public void UpdateStateMachine();
    public void ChangeState(int previousState, int nextState);
}
