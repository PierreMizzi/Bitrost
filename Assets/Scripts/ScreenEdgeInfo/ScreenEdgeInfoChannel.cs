using UnityEngine;

namespace Bitfrost.Gameplay.ScreenEdgeInfo
{
	public delegate void InitiliazeSubject(ScreenEdgeSubject subject);

	[CreateAssetMenu(fileName = "ScreenEdgeInfoChannel", menuName = "Bitrost/ScreenEdgeInfoChannel", order = 0)]
	public class ScreenEdgeInfoChannel : ScriptableObject
	{

		public InitiliazeSubject onInitializeSubject;

		private void OnEnable()
		{
			onInitializeSubject = (ScreenEdgeSubject subject) => { };
		}
	}
}