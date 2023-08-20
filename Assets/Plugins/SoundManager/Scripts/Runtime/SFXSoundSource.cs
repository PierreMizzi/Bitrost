using UnityEngine;
using System;

namespace PierreMizzi.SoundManager
{
	[RequireComponent(typeof(AudioSource))]
	public class SFXSoundSource : MonoBehaviour
	{

		#region Fields 

		private AudioSource m_audioSource;
		public Action onAudioClipEnded;

		private SoundData m_currentSoundData;
		public SoundData CurrentSoundData { get { return m_currentSoundData; } }


		#endregion

		#region Methods 

		protected virtual void Awake()
		{
			CheckAudioSource();
			m_audioSource.loop = false;
		}

		protected virtual void Update()
		{
			if (m_audioSource.clip != null)
			{
				if ((m_audioSource.clip.length - m_audioSource.time) < 0.01f)
					AudioClipEnded();
			}
		}

		/// <summary>
		/// Play a SoundData
		/// </summary>
		/// <param name="data">Given SoundData</param>
		public void Play(SoundData data)
		{
			CheckAudioSource();

			SetSoundData(data);

			if (m_audioSource.clip != null)
				m_audioSource.Play();
		}

		/// <summary>
		/// Play a SoundData given its ID
		/// </summary>
		/// <param name="data">Given SoundData</param>
		public void Play(string soundDataID)
		{
			SetSoundData(soundDataID);

			if (m_audioSource.clip != null)
				m_audioSource.Play();
		}

		/// <summary>
		/// Set a SoundData to play
		/// </summary>
		/// <param name="data">Given SoundData</param>
		public void SetSoundData(SoundData data)
		{
			CheckAudioSource();

			if (data != null && data.Clip != null)
			{
				m_currentSoundData = data;
				m_audioSource.clip = m_currentSoundData.Clip;
				m_audioSource.outputAudioMixerGroup = m_currentSoundData.Mixer;
			}
		}

		public void SetSoundData(string ID)
		{
			SoundData data = SoundManager.GetSoundData(ID);

			SetSoundData(data);
		}

		public void UnsetSoundData()
		{
			m_currentSoundData = null;
			m_audioSource.clip = null;
			m_audioSource.outputAudioMixerGroup = null;
		}

		private void CheckAudioSource()
		{
			if (m_audioSource == null)
				m_audioSource = GetComponent<AudioSource>();
		}

		protected virtual void AudioClipEnded()
		{
			UnsetSoundData();
			onAudioClipEnded?.Invoke();
			SoundManager.ReleaseSFXSSToPool(this);
		}

		#endregion

	}
}