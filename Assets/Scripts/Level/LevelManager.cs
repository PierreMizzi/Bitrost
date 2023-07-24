using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private float m_arenaDiameter = 10f;

    private static float m_arenaRadiusSqr = 0f;

	#endregion

	#region Methods

    private void Awake()
    {
        m_arenaRadiusSqr = math.pow(m_arenaDiameter / 2f, 2f);
    }

    public static bool IsInsideArena(Vector3 position)
    {
        return position.sqrMagnitude < m_arenaRadiusSqr;
    }

	#endregion
}
