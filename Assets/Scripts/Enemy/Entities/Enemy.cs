using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEntity))]
public class Enemy : MonoBehaviour, IStateMachine
{
	#region Fields

    [SerializeField]
    protected EnemyType m_type = EnemyType.None;
    public EnemyType type
    {
        get { return m_type; }
    }

    private HealthEntity m_healthEntity;

    #region StateMachine

    public List<AState> states { get; set; } = new List<AState>();
    public AState currentState { get; set; }

    #endregion

	#endregion

	#region Methods

    protected virtual void Awake()
    {
        m_healthEntity = GetComponent<HealthEntity>();
        InitiliazeState();

        ChangeState(EnemyStateType.None, EnemyStateType.Idle);
    }

    protected virtual void Update()
    {
        UpdateStateMachine();
    }

    #region StateMachine

    public virtual void InitiliazeState()
    {
        states = new List<AState>()
        {
            new EnemyIdleState(this),
            new EnemyMoveState(this),
            new EnemyAttackState(this)
        };
    }

    public void ChangeState(EnemyStateType previousState, EnemyStateType nextState)
    {
        ChangeState((int)previousState, (int)nextState);
    }

    public virtual void ChangeState(int previousState, int nextState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = states.Find((AState newState) => newState.type == nextState);
        if (currentState != null)
            currentState.Enter(previousState);
        else
        {
            Debug.LogError($"Couldn't find a new state of type : {nextState}. Going GroundedIdle");
        }
    }

    public void UpdateStateMachine()
    {
        if (currentState != null)
            currentState.Update();
    }

	#endregion



	#endregion
}
