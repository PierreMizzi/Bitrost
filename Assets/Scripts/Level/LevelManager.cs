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

    private bool isStageCompleted;

    private bool m_isStageDurationOver;

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

    private void Awake()
    {
        m_director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        if (m_levelChannel != null)
            m_levelChannel.onAllEnemiesKilled += CallbackAllEnemiesKilled;
    }

    private void OnDestroy()
    {
        if (m_levelChannel != null)
            m_levelChannel.onAllEnemiesKilled -= CallbackAllEnemiesKilled;
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

    #region Stage Management

    private void PlayStage()
    {
        m_director.Play();
        m_isStageDurationOver = false;
        isStageCompleted = false;
    }

    // From Signal Emitter
    public void CallbackStayStage()
    {
        m_isStageDurationOver = true;

        if (!isStageCompleted)
            m_director.Pause();
        else
            PlayStage();
    }

    public void CallbackAllEnemiesKilled()
    {
        isStageCompleted = true;

        if (m_isStageDurationOver)
            PlayStage();
    }

    #endregion

	#endregion
}
