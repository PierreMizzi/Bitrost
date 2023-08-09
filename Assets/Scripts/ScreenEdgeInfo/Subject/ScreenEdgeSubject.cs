using System.Collections;
using UnityEngine;

namespace Bitfrost.Gameplay.ScreenEdgeInfo
{

	public class ScreenEdgeSubject : MonoBehaviour
	{
		#region Fields 

		[SerializeField]
		protected ScreenEdgeInfoChannel m_infoChannel;

		protected ScreenEdgeInfoManager m_manager = null;

		[SerializeField]
		protected ScreenEdgeRenderer m_screenEdgeRenderer = null;

		/*
			All values below are in Screen Space, 
			with the middle of the screen as the origin
		*/

		protected Vector3 m_position;
		protected float m_magnitudeToSelf;

		/// <summary> 
		/// In Radians
		/// </summary> 
		public float angle { get; private set; }
		public Vector3 direction { get; private set; }
		public float magnitudeToEdge { get; private set; }

		public bool isOutOfScreen { get; private set; }

		[SerializeField]
		protected bool m_destroyOnScreen;

		#endregion

		#region Methods 

		#region MonoBehaviour

		protected virtual IEnumerator Start()
		{
			yield return new WaitForSeconds(0.2f);

			if (m_infoChannel != null)
				m_infoChannel.onInitializeSubject.Invoke(this);
		}

		protected void LateUpdate()
		{
			ComputeProperties();
		}

		#endregion

		public virtual void Initialize(ScreenEdgeInfoManager manager)
		{
			m_manager = manager;

			// Renderer
			m_screenEdgeRenderer.Initialize(manager, this);



		}

		protected void ComputeProperties()
		{
			if (m_manager != null)
			{
				m_position = m_manager.camera.WorldToScreenPoint(transform.position);

				direction = (m_position - m_manager.screenCenter).normalized;
				m_magnitudeToSelf = (m_position - m_manager.screenCenter).magnitude;

				angle = Mathf.Atan2(direction.y, direction.x);
				magnitudeToEdge = m_manager.MagnitudeToEdge(angle);
				isOutOfScreen = magnitudeToEdge < m_magnitudeToSelf;
			}
		}

		#endregion
	}
}