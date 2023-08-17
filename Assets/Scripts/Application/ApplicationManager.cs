using UnityEngine;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.SceneManagement;

namespace Bitfrost.Application
{
	public class ApplicationManager : BaseAppManager
	{
		#region Fields 

		[SerializeField]
		private SoundManagerToolSettings m_soundManagerSettings = null;

		#endregion

		#region Methods 

		protected override void Start()
		{
			SoundManager.PlaySound(SoundDataID.GAME_LOOP, true);
		}

		#endregion
	}
}