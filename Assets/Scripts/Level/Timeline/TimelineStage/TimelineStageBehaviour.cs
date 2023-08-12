using UnityEngine.Playables;
using Bitfrost.Gameplay.Energy;

namespace Bitfrost.Gameplay
{
    public class TimelineStageBehaviour : PlayableBehaviour
    {
        private bool m_isDone;

        public LevelChannel levelChannel;
        public int stageDifficulty;
        public SpawnCrystalShardsConfig spawnCrystalConfig;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!m_isDone)
            {

                // m_manager = playerData as CrystalShardsManager;
                levelChannel.crystalManager.SpawnCrystalShards(spawnCrystalConfig);

                levelChannel.onChangeStageDifficulty.Invoke(stageDifficulty);

                m_isDone = true;
            }
        }
    }
}
