using UnityEngine.Playables;

public class SpawnCrystalShardsBehaviour : PlayableBehaviour
{
    private bool m_isDone;

    public SpawnCrystalShardsConfig spawnConfig;

	public CrystalShardsManager m_manager = null;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!m_isDone)
        {
            m_manager = playerData as CrystalShardsManager;
            m_manager.SpawnCrystalShards(spawnConfig);

            m_isDone = true;
        }
    }
}
