using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private float m_arenaDiameter = 10f;

    public static float arenaRadius;

    public static float arenaRadiusSqr = 0f;

    [SerializeField]
    private LevelChannel m_levelChannel = null;

    #region Time

    public float time { get; private set; }

    #endregion

    #region Timeline

    private PlayableDirector m_director;

    #endregion

	#endregion

	#region Methods

    private void OnEnable()
    {
        arenaRadius = m_arenaDiameter / 2f;
        arenaRadiusSqr = math.pow(arenaRadius, 2f);
    }

    private void Update()
    {
        time += Time.unscaledDeltaTime;
    }

    public static bool IsInsideArena(Vector3 position)
    {
        return position.sqrMagnitude < arenaRadiusSqr;
    }

    public static Vector3 RandomPosition()
    {
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        float randomLength = UnityEngine.Random.Range(0f, 1f);

        return new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0)
            * arenaRadius
            * randomLength;
    }

	#endregion
}
