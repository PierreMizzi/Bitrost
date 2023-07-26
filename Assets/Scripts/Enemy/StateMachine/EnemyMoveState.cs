public class EnemyMoveState : AState
{
    public EnemyMoveState(IStateMachine stateMachine)
        : base(stateMachine)
    {
        type = (int)EnemyStateType.Move;
    }
}
