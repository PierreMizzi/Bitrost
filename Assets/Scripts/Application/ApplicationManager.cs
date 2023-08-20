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

		[SerializeField]
		private Transform m_soundSourceContainer = null;

		#endregion

		#region Methods 

		protected override void OnEnable()
		{
			base.OnEnable();
			SaveManager.LoadSaveData();
		}

		protected override void Start()
		{
			base.Start();
			applicationChannel.onSetCursor.Invoke(CursorType.Normal);
			// if (UnityEngine.Application.isPlaying)
			// 	SoundManager.PlaySound(SoundDataID.GAME_LOOP, true);
		}

		#endregion

		#region Save System

		[ContextMenu("Load Save Data")]
		public void LoadSaveData()
		{
			SaveManager.LoadSaveData();
		}

		[ContextMenu("Write Save Data")]
		public void WriteSaveData()
		{
			SaveManager.WriteSaveData();
		}

		[ContextMenu("Log Save Data")]
		public void LogSaveData()
		{
			SaveManager.LogSaveData();
		}

		#endregion
	}
}