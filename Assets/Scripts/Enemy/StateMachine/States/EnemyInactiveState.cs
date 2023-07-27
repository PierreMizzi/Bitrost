public class EnemyInactiveState : AState
{
    public EnemyInactiveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        type = (int)EnemyStateType.Inactive;
    }
}