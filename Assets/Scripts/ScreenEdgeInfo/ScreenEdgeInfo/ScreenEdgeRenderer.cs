using System;
using UnityEngine;

namespace Bitfrost.Gameplay.ScreenEdgeInfo
{

	public class ScreenEdgeRenderer : MonoBehaviour
	{
		#region Fields 

		protected ScreenEdgeInfoManager m_manager = null;

		protected ScreenEdgeSubject m_subject = null;

		[SerializeField]
		protected GameObject m_rendererObject = null;

		protected Vector3 m_positionScreenEdge;
		protected Vector3 m_positionElipse;
		protected Vector3 m_lerpedPosition;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_edgeElipseLerp;

		[SerializeField]
		protected float m_elipseSize = 0.8f;
		protected float m_elipseHorizontalSize;
		protected float m_elipseVerticalSize;

		#endregion

		#region Methods 

		#region MonoBehaviour

		protected virtual void LateUpdate()
		{
			UpdatePosition();
			UpdateVisibility();
		}

		#endregion

		#region Behaviour

		public virtual void Initialize(ScreenEdgeInfoManager manager, ScreenEdgeSubject subject)
		{
			m_manager = manager;
			m_subject = subject;
			ComputeElipseSize();
		}

		protected virtual void UpdatePosition()
		{
			if (m_manager != null && m_subject.isOutOfScreen)
			{
				ComputeElipseSize();

				m_positionScreenEdge = m_subject.magnitudeToEdge * m_subject.direction;
				m_positionElipse = GetPositionOnElipse(m_subject.angle);
				m_lerpedPosition = Vector3.Lerp(m_positionScreenEdge, m_positionElipse, m_edgeElipseLerp);

				m_lerpedPosition = m_manager.camera.ScreenToWorldPoint(m_manager.screenCenter + m_lerpedPosition);
				m_lerpedPosition.z = 0;
				transform.position = m_lerpedPosition;
			}
		}

		protected virtual void UpdateVisibility()
		{
			if (m_subject != null)
				m_rendererObject.SetActive(m_subject.isOutOfScreen);
		}

		#endregion

		#region Elipse

		protected void ComputeElipseSize()
		{
			if (m_manager != null)
			{
				m_elipseHorizontalSize = m_elipseSize * (m_manager.camera.pixelWidth / 2f);
				float horizontalPixelOffset = (m_manager.camera.pixelWidth / 2f) - m_elipseHorizontalSize;

				m_elipseVerticalSize = (m_manager.camera.pixelHeight / 2f) - horizontalPixelOffset;
			}
		}

		protected Vector3 GetPositionOnElipse(float angle)
		{
			return new Vector3(m_elipseHorizontalSize * Mathf.Cos(angle), m_elipseVerticalSize * Mathf.Sin(angle), 0f);
		}

		#endregion

		#endregion
	}
}