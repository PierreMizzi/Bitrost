using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private float m_arenaDiameter = 10f;

    public static float arenaRadius;

    public static float arenaRadiusSqr = 0f;

	#endregion

	#region Methods

    private void OnEnable()
    {
        arenaRadius = m_arenaDiameter /2f;
        arenaRadiusSqr = math.pow(arenaRadius, 2f);
    }

    public static bool IsInsideArena(Vector3 position)
    {
        return position.sqrMagnitude < arenaRadiusSqr;
    }

    public static Vector3 RandomPosition()
    {
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);

        return new Vector3(randomX, randomY, 0) * arenaRadius;
    }

	#endregion
}
