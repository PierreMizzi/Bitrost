using UnityEngine;
using UnityEngine.Playables;

public class LevelManagerAsset : PlayableAsset
{
    public ExposedReference<LevelChannel> m_levelChannel;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LevelManagerBehaviour>.Create(graph);

        LevelManagerBehaviour behaviour = playable.GetBehaviour();
		
        behaviour.levelChannel = m_levelChannel.Resolve(graph.GetResolver());

        return playable;
    }
}
