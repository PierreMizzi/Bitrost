using UnityEngine.Playables;

public class EnemySpawnerBehaviour : PlayableBehaviour
{
    private EnemyManager m_manager;
    public bool m_done = false;

    public EnemySpawnConfig spawnConfig;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!m_done)
        {
            m_manager = playerData as EnemyManager;
            m_manager.ChangeEnemySpawnConfig(spawnConfig);

			m_done = true;
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // Debug.Log("OnBehaviourPlay");
    }

	#region Test Behaviour

    // public override void PrepareData(Playable playable, FrameData info)
    // {
    //     Debug.Log("PrepareData");
    // }

    // public override void PrepareFrame(Playable playable, FrameData info)
    // {
    //     Debug.Log("PrepareFrame");
    // }

    // public override void OnBehaviourPause(Playable playable, FrameData info)
    // {
    //     Debug.Log("OnBehaviourPause");
    // }

    // public override void OnGraphStart(Playable playable)
    // {
    //     Debug.Log("OnGraphStart");
    // }

    // public override void OnGraphStop(Playable playable)
    // {
    //     Debug.Log("OnGraphStop");
    // }

    // public override void OnPlayableCreate(Playable playable)
    // {
    //     Debug.Log("OnPlayableCreate");
    // }

    // public override void OnPlayableDestroy(Playable playable)
    // {
    //     Debug.Log("OnPlayableDestroy");
    // }

	#endregion
}
