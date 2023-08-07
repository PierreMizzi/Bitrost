using UnityEngine.Timeline;

namespace Bitfrost.Gameplay.Enemies
{

	[TrackClipType(typeof(EnemySpawnerAsset))]
	[TrackBindingType(typeof(EnemyManager))]
	[TrackColor(1f, 0f, 0f)]
	public class EnemySpawnerTrack : TrackAsset { }
}
