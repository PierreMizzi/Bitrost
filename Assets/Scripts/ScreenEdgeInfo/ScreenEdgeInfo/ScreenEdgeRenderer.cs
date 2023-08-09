/*

	ScreenEdgeInfo


*/

using System;
using UnityEngine;
namespace Bitfrost.Gameplay.ScreenEdgeInfo
{

	public class ScreenEdgeRenderer : MonoBehaviour
	{
		#region Fields 

		protected ScreenEdgeInfoManager m_manager = null;

		protected ScreenEdgeSubject m_subject = null;



		[Obsolete]
		private Vector3 m_subjectScreenPosition;

		[Obsolete]
		protected Vector3 m_cameraToSubject;

		[Obsolete]
		protected float m_cameraToSubjectMagnitude;

		/// <summary> 
		/// In Radians
		[Obsolete]
		protected float m_cameraToSubjectAngle;

		[SerializeField]
		[Obsolete]
		private bool m_isOutOfScreen;

		[SerializeField]
		private GameObject m_renderer = null;

		private Vector3 m_positionScreenEdge;
		private Vector3 m_positionElipse;
		private Vector3 m_lerpedPosition;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_edgeElipseLerp;

		[SerializeField]
		private float m_elipseSize = 0.8f;
		private float m_elipseHorizontalSize;
		private float m_elipseVerticalSize;

		#endregion

		#region Methods 

		#region MonoBehaviour

		private void LateUpdate()
		{
			UpdatePosition();
			UpdateVisibility();
		}

		#endregion

		#region Behaviour

		public void Initialize(ScreenEdgeInfoManager manager, ScreenEdgeSubject subject)
		{
			m_manager = manager;
			m_subject = subject;
			ComputeElipseSize();
			Debug.Log("Renderer Initialized");
		}

		[SerializeField]
		private float magnitudeToEdge;

		protected void ComputeProperties()
		{
			if (m_manager != null)
			{
				m_subjectScreenPosition = m_manager.camera.WorldToScreenPoint(m_subject.transform.position);

				m_cameraToSubject = (m_subjectScreenPosition - m_manager.screenCenter).normalized;
				m_cameraToSubjectMagnitude = (m_subjectScreenPosition - m_manager.screenCenter).magnitude;

				m_cameraToSubjectAngle = Mathf.Atan2(m_cameraToSubject.y, m_cameraToSubject.x);
				magnitudeToEdge = m_manager.MagnitudeToEdge(m_cameraToSubjectAngle);
				m_isOutOfScreen = m_manager.MagnitudeToEdge(m_cameraToSubjectAngle) < m_cameraToSubjectMagnitude;
			}
		}



		protected virtual void UpdatePositioOld()
		{
			if (m_manager != null && m_isOutOfScreen)
			{
				ComputeElipseSize();

				m_positionScreenEdge = magnitudeToEdge * m_cameraToSubject;
				m_positionElipse = GetPositionOnElipse(m_cameraToSubjectAngle);
				m_lerpedPosition = Vector3.Lerp(m_positionScreenEdge, m_positionElipse, m_edgeElipseLerp);

				m_lerpedPosition = m_manager.camera.ScreenToWorldPoint(m_manager.screenCenter + m_lerpedPosition);
				m_lerpedPosition.z = 0;
				transform.position = m_lerpedPosition;
			}
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
				m_renderer.SetActive(m_subject.isOutOfScreen);
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