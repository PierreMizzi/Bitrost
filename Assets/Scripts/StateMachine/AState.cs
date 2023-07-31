using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class for Player's state
/// </summary>
public abstract class AState
{
    protected IStateMachine m_stateMachine = null;

    public int type { get; protected set; }

    public bool canUpdate = true;

    public void Initialize(IStateMachine stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    /// <summary>
    /// Dictionnary to store specific "EnterState" mode when transitioning from one state to another
    /// </summary>
    protected Dictionary<int, Action> m_stateTransitions = new Dictionary<int, Action>();

    public AState(IStateMachine stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public void ChangeState(int nextState)
    {
        m_stateMachine.ChangeState(type, nextState);
    }

    // State Machine lifecycle
    protected virtual void DefaultEnter()
    {
        // Debug.Log($"ENTER : {type}");
    }

    /// <summary>
    /// Seach for an "Enter" method matching previous state, if none, DefaultEnter is called
    /// </summary>
    public virtual void Enter(int previousState)
    {
        if (m_stateTransitions.ContainsKey(previousState))
            m_stateTransitions[previousState]();
        else
            DefaultEnter();
    }

    public virtual void Update() { }

    public virtual void Exit() { }

    protected virtual void Pause() { }

    protected virtual void Unpause() { }
}
