/*

	ScreenEdgeInfo


*/

using UnityEngine;

public class AScreenEdgeInfo : MonoBehaviour
{
	#region Fields 

	protected ScreenEdgeInfoManager m_manager = null;

	[SerializeField] protected ScreenEdgeInfoType m_type;
	public ScreenEdgeInfoType type { get { return m_type; } set { m_type = value; } }

	protected AInfoSubject m_subject = null;

	[SerializeField] private bool m_destroyWhenEnterSceen;


	[SerializeField]
	private Vector3 m_subjectScreenPosition;

	[SerializeField]
	protected Vector3 m_cameraToSubject;

	[SerializeField]
	protected float m_cameraToSubjectMagnitude;

	/// <summary> 
	/// In Radians
	[SerializeField] /// </summary>
	protected float m_cameraToSubjectAngle;

	[SerializeField]
	public bool m_isOutOfScreen;

	[SerializeField]
	private GameObject m_renderer = null;



	#endregion

	#region Methods 

	#region MonoBehaviour

	private void LateUpdate()
	{
		ComputeProperties();
		UpdatePosition();
		UpdateVisibility();
	}

	private void OnDrawGizmos()
	{

		Vector3 cameraPosition = Camera.main.transform.position;
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(cameraPosition, cameraPosition + m_cameraToSubject * magnitudeToEdge);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(cameraPosition, cameraPosition + m_cameraToSubject * m_cameraToSubjectMagnitude);
	}

	#endregion

	#region Behaviour

	public void Initialize(ScreenEdgeInfoManager manager, AInfoSubject subject)
	{
		m_manager = manager;
		m_subject = subject;
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

	[SerializeField] private float m_edgeScreenOffset = 0.8f;

	protected virtual void UpdatePosition()
	{
		if (m_manager != null && m_isOutOfScreen)
		{
			transform.position = m_manager.camera.transform.position +
								 m_manager.camera.ScreenToWorldPoint(m_manager.screenCenter + m_edgeScreenOffset * magnitudeToEdge * m_cameraToSubject);
		}
	}

	protected virtual void UpdateVisibility()
	{
		if (m_manager != null)
			m_renderer.SetActive(m_isOutOfScreen);
	}



	#endregion

	#endregion
}