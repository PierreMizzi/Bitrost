using UnityEngine;
using PierreMizzi.SoundManager;

public class ApplicationManager : MonoBehaviour
{
	#region Fields 

	[SerializeField]
	private SoundManagerToolSettings m_soundManagerSettings = null;

	#endregion

	#region Methods 

	private void Start()
	{
		SoundManager.PlaySound(SoundDataIDStatic.GAME_LOOP, true);
	}

	#endregion
}