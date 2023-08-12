using UnityEngine;
using UnityEngine.Playables;
using Bitfrost.Gameplay.Energy;

namespace Bitfrost.Gameplay
{
    public class TimelineStageAsset : PlayableAsset
    {

        [SerializeField]
        private LevelChannel m_levelChannel;

        [Header("Stage")]
        [SerializeField]
        private int m_stageDifficulty = 1;

        [Header("Crystals")]
        [SerializeField]
        private SpawnCrystalShardsConfig m_spawnCrystalConfig;
        public LevelChannel levelChannel { get { return m_levelChannel; } }
        public SpawnCrystalShardsConfig spawnCrystalConfig { get { return m_spawnCrystalConfig; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TimelineStageBehaviour>.Create(graph);

            TimelineStageBehaviour behaviour = playable.GetBehaviour();

            behaviour.levelChannel = m_levelChannel;
            behaviour.stageDifficulty = m_stageDifficulty;
            behaviour.spawnCrystalConfig = spawnCrystalConfig;

            return playable;
        }
    }
}
