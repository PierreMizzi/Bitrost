using UnityEngine;

[RequireComponent(typeof(HealthEntity))]
public class Enemy : MonoBehaviour
{
	#region Fields

    [SerializeField]
    protected EnemyType m_type = EnemyType.None;
    public EnemyType type
    {
        get { return m_type; }
    }

	#endregion

	#region Methods

	#endregion
}
