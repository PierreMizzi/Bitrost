using Bitfrost.Gameplay;
using PierreMizzi.Useful.SceneManagement;
using UnityEngine;

namespace Bitfrost.Application
{

	[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "Overcore/Channels/Architecture/Application Channel", order = 0)]
	public class ApplicationChannel : BaseAppChannel
	{

		public CursorDelegate onSetCursor;

		protected override void OnEnable()
		{
			base.OnEnable();
			onSetCursor = (CursorType type) => { };
		}

	}
}