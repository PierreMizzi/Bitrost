using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
    #region Fields

    [Header("Channels")]

    [SerializeField]
    private ApplicationChannel m_appChannel = null;

    [SerializeField]
    private LevelChannel m_levelChannel = null;

    [SerializeField]
    private float m_arenaDiameter = 10f;

    public static float arenaRadius;

    public static float arenaRadiusSqr = 0f;



    private bool isStageCompleted;

    private bool m_isStageDurationOver;

    #region Time

    public float time { get; private set; }

    public bool m_updateTime = false;

    #endregion

    #region Timeline

    private PlayableDirector m_director;

    #endregion

    #endregion

    #region Methods

    #region MonoBehaviour

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
        {
            m_levelChannel.onAllEnemiesKilled += CallbackAllEnemiesKilled;
            m_levelChannel.onGameOver += CallbackGameOver;
            m_levelChannel.onReset += CallbackReset;
        }

        m_updateTime = true;
    }

    private void Update()
    {
        if (m_updateTime)
            time += Time.unscaledDeltaTime;
    }

    private void OnDestroy()
    {
        if (m_levelChannel != null)
        {
            m_levelChannel.onAllEnemiesKilled -= CallbackAllEnemiesKilled;
            m_levelChannel.onGameOver -= CallbackGameOver;
            m_levelChannel.onReset -= CallbackReset;

        }
    }

    #endregion

    #region Arena

    public static bool IsInsideArena(Vector3 position)
    {
        return position.sqrMagnitude < arenaRadiusSqr;
    }

    public static Vector3 RandomPosition(Vector3 origin, float radius)
    {
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        float randomLength = UnityEngine.Random.Range(0f, 1f);

        return origin + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0)
            * radius
            * randomLength;
    }

    public static List<Vector3> RandomPositions(int count, float radius)
    {
        Vector3 origin = RandomPosition(Vector3.zero, arenaRadius - radius);

        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < count; i++)
            positions.Add(RandomPosition(origin, radius));


        return positions;
    }

    #endregion

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

    private void ResetStage()
    {
        m_director.Stop();
        m_director.Play();
    }

    #endregion

    #region Name

    #endregion

    #region Game Over

    /*
    
        // TODO
        // TODO : Highscore
                    -> Track kill count, score per enemy type
        // TODO : Pause the game
                    -> LevelManager
                        -> Pause the time
        // TODO : Show GAME OVER Screen                                 DONE
                    -> Highscore                                    
                    -> Time                                         DONE
                    -> Restart                                      WIP
                    -> Menu                                         

        // TODO : Restart
                    -> Player                                       DONE
                        -> Position                                 DONE
                        -> Health                                   DONE
                    -> Camera
                        -> Position
                    -> Module Manager                               DONE
                        -> RemainingModule back to 1                DONE
                        -> Retrieve Module                          DONE
                        -> Reset Module Stored Energy               DONE
                    -> EnemyManager                                 DONE
                        -> Release enemies                          DONE
                        -> Empty pools                              DONE
                    -> BulletManager
                        -> Release bullet                           DONE
                        -> Empty pools                              ????
                    -> CrystalShardsManager                         DONE
                        -> Release crystals                         DONE
                        -> Empty pools                              ????
        // TODO : Menu
                    -> Unload scene



        if (m_levelChannel != null)
            m_levelChannel.onReset += CallbackReset;
        if (m_levelChannel != null)
            m_levelChannel.onReset -= CallbackReset;

        #region Reset

        public void CallbackReset()
        {

        }

        #endregion

    */

    private void CallbackGameOver()
    {
        GameOverData data = new GameOverData(time);

        m_appChannel.onSetCursor.Invoke(CursorType.Normal);

        m_levelChannel.onGameOverPanel.Invoke(data);
    }

    private void CallbackReset()
    {
        // Time
        time = 0;
        m_updateTime = true;

        // Stage
        ResetStage();
    }

    #endregion

    #endregion
}
