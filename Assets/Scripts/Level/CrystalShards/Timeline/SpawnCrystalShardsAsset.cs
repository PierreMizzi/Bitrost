using UnityEngine;
using UnityEngine.Playables;

namespace Bitfrost.Gameplay.Energy
{
    public class SpawnCrystalShardsAsset : PlayableAsset
    {
        public SpawnCrystalShardsConfig spawnConfig;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SpawnCrystalShardsBehaviour>.Create(graph);

            SpawnCrystalShardsBehaviour behaviour = playable.GetBehaviour();

            behaviour.spawnConfig = spawnConfig;

            return playable;
        }
    }
}
