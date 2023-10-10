using UnityEngine;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.SceneManagement;
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
			SoundManager.Init(m_soundManagerSettings.path);
			SaveManager.LoadSaveData();
			applicationChannel.onSetCursor.Invoke(CursorType.Normal);

			// Start loading the scene
			base.Start();
		}

		#endregion
	}
}