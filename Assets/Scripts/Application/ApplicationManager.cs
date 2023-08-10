using UnityEngine;
using PierreMizzi.SoundManager;

namespace Bitfrost.Application
{
	public class ApplicationManager : MonoBehaviour
	{
		#region Fields 

		[SerializeField]
		private SoundManagerToolSettings m_soundManagerSettings = null;

		#endregion

		#region Methods 

		private void Start()
		{
			SoundManager.PlaySound(SoundDataID.GAME_LOOP, true);
		}

		#endregion
	}
}