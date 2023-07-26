using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(HealthEntity))]
public class Player : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private LevelChannel m_levelChannel = null;

	#endregion

	#region Methods

    private void Start()
    {
        m_levelChannel.player = gameObject;
    }

	#endregion
}
