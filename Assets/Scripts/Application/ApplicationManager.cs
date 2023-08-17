using UnityEngine;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.SceneManagement;
using System;
using Bitfrost.Gameplay;

namespace Bitfrost.Application
{
	public class ApplicationManager : BaseAppManager
	{
		#region Fields 

		public ApplicationChannel applicationChannel { get { return m_appChannel as ApplicationChannel; } }

		[SerializeField]
		private SoundManagerToolSettings m_soundManagerSettings = null;

		#endregion

		#region Methods 

		protected override void Start()
		{
			base.Start();
			applicationChannel.onSetCursor.Invoke(CursorType.Normal);
			if (UnityEngine.Application.isPlaying)
				SoundManager.PlaySound(SoundDataID.GAME_LOOP, true);
		}

		#endregion
	}
}