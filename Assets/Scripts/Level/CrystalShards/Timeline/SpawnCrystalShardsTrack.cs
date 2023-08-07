using UnityEngine;
using UnityEngine.Timeline;


namespace Bitfrost.Gameplay.Energy
{
	[TrackClipType(typeof(SpawnCrystalShardsAsset))]
	[TrackBindingType(typeof(CrystalShardsManager))]
	[TrackColor(0.53f, 1, 0.97f)]
	public class SpawnCrystalShardsTrack : PlayableTrack { }
}