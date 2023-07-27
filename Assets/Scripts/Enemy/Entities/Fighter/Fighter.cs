using System.Collections.Generic;
using UnityEngine;

public class Fighter : Enemy
{
	#region Fields

	#endregion

	#region Methods

	#region State Machine

    public override void InitiliazeState()
    {
        states = new List<AState>()
        {
            new EnemyInactiveState(this),
            new EnemyIdleState(this),
            new FighterMoveState(this),
            new FighterAttackState(this)
        };
    }

	#endregion

	#endregion
}
