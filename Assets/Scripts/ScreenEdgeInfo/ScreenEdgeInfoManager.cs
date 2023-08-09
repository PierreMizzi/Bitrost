using System;
using UnityEngine;

namespace Bitfrost.Gameplay.ScreenEdgeInfo
{
	public class ScreenEdgeInfoManager : MonoBehaviour
	{
		#region Fields 

		[SerializeField]
		private ScreenEdgeInfoChannel m_infoChannel;
		public new Camera camera { get; private set; }
		public Vector3 screenCenter { get; private set; }

		[SerializeField]
		private float m_cameraSizeOffset;

		#endregion

		#region Methods 

		#region MonoBehaviour

		private void Awake()
		{
			camera = Camera.main;
			screenCenter = new Vector3(camera.pixelWidth / 2f, camera.pixelHeight / 2f, 0f);
		}

		private void Start()
		{
			if (m_infoChannel != null)
				m_infoChannel.onInitializeSubject += CallbackInitializeSubject;
		}

		private void OnDestroy()
		{
			if (m_infoChannel != null)
				m_infoChannel.onInitializeSubject -= CallbackInitializeSubject;
		}



		#endregion

		#region Behaviour

		private void CallbackInitializeSubject(ScreenEdgeSubject subject)
		{
			subject.Initialize(this);
		}

		#endregion

		public float MagnitudeToEdge(float angle)
		{
			return camera.MagnitudeToEdge(angle, m_cameraSizeOffset);
		}

		#endregion
	}
}