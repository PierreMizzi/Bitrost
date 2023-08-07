using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.SoundManager
{
	[CreateAssetMenu(fileName = "SoundManagerToolSettings", menuName = "Extensions/AudioManager/SoundManagerToolSettings", order = 1)]
	public class SoundManagerToolSettings : ScriptableObject
	{

		#region Fields

		#region Settings

		[Header("AudioMixer")]
		[SerializeField] private List<AudioMixerController> m_audioMixerControllers = new List<AudioMixerController>();

		[Header("SoundData Libraries")]
		[Tooltip("Base duration when fading in a sound")]
		[SerializeField] private float m_baseFadeInDuration = 1f;

		[Tooltip("Base duration when fading out a sound")]
		[SerializeField] private float m_baseFadeOutDuration = 1f;

		[SerializeField] private List<SoundDataLibrary> m_soundDataLibraries = null;
		public List<AudioMixerController> AudioMixerControllers { get { return m_audioMixerControllers; } }

		public float BaseFadeInDuration { get { return m_baseFadeInDuration; } }
		public float BaseFadeOutDuration { get { return m_baseFadeOutDuration; } }
		public List<SoundDataLibrary> SoundDataLibraries { get { return m_soundDataLibraries; } }

		#endregion

		#region SoundDataID static class generation

		#endregion

		[SerializeField] private string _path = "";

		#endregion

		#region Methods

		#region Scriptable Object Behaviour

		private void OnEnable()
		{
			SoundManager.Init(_path);
		}

		#endregion

		#endregion


	}

}