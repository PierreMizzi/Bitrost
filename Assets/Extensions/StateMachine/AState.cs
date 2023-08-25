using System;
using System.Collections.Generic;
using PierreMizzi.Pause;
using UnityEngine;

namespace PierreMizzi.Useful.StateMachines
{


    /// <summary>
    /// Base abstract class for Player's state
    /// </summary>
    public abstract class AState : IPausable
    {
        protected IStateMachine m_stateMachine = null;

        public int type { get; protected set; }

        public bool isPaused { get; set; }

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

        public virtual void Pause()
        {
            isPaused = true;
        }

        public virtual void Resume()
        {
            isPaused = false;
        }
    }
}
