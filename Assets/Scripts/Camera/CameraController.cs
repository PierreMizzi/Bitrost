using Bitfrost.Gameplay;
using PierreMizzi.Useful;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour, IPausable
{
	#region Fields

	[SerializeField]
	private LevelChannel m_levelChannel;

	[SerializeField]
	private Transform m_target = null;

	private Vector3 m_targetPosition;

	private Camera m_camera = null;

	#region Mouse Position 

	[Header("Inputs")]
	[SerializeField]
	private InputActionReference m_mousePositionActionReference = null;

	private Vector3 m_mousePositionValue;

	private Vector3 mousePositionScreenCenter
	{
		get
		{
			return new Vector3(
				m_mousePositionValue.x - m_camera.pixelWidth / 2f,
				m_mousePositionValue.y - m_camera.pixelHeight / 2f
			);
		}
	}

	#endregion

	#region Screen Edge panning

	[SerializeField]
	private float m_treshold = 0.75f;

	[SerializeField]
	private float m_offsetMagnitude = 1f;

	[SerializeField]
	private float m_smoothDampSpeed;

	[SerializeField]
	private float m_smoothDampMaxSpeed;

	private float m_offset;

	private Vector3 velocity;

	private Vector3 m_cameraOffsetPosition;
	private Vector3 m_previousCameraOffsetPosition;

	#endregion

	public bool isPaused { get; set; }

	#endregion

	#region Methods

	#region MonoBehaviour

	private void Awake()
	{
		m_camera = Camera.main;
	}

	private void Start()
	{
		if (m_levelChannel != null)
		{
			m_levelChannel.onPauseGame += Pause;
			m_levelChannel.onResumeGame += Resume;

		}
	}

	private void Update()
	{
		if (!isPaused)
			m_mousePositionValue = m_mousePositionActionReference.action.ReadValue<Vector2>();
	}

	private void LateUpdate()
	{
		if (!isPaused)
		{
			m_targetPosition = m_target.position;
			m_targetPosition.z = -1;

			ManageCameraOffset();
			transform.position = m_targetPosition + m_cameraOffsetPosition;
		}
	}

	private void OnDestroy()
	{
		if (m_levelChannel != null)
		{
			m_levelChannel.onPauseGame -= Pause;
			m_levelChannel.onResumeGame -= Resume;
		}
	}

	#endregion

	#region Screen Edge panning

	private void ManageCameraOffset()
	{
		float angle = Mathf.Atan2(mousePositionScreenCenter.y, mousePositionScreenCenter.x);

		float edgeMagnitude = MagnitudeFromAngle(angle);

		float thresholdEdgeMagnitude = edgeMagnitude * m_treshold;

		float mousePositionMangitude = mousePositionScreenCenter.magnitude;

		m_offset = UtilsClass.Remap(
			mousePositionMangitude,
			thresholdEdgeMagnitude,
			edgeMagnitude,
			0f,
			1f
		);
		m_offset = Mathf.Clamp01(m_offset);

		m_cameraOffsetPosition =
			mousePositionScreenCenter.normalized * m_offset * m_offsetMagnitude;

		m_cameraOffsetPosition = Vector3.SmoothDamp(
			m_previousCameraOffsetPosition,
			m_cameraOffsetPosition,
			ref velocity,
			m_smoothDampSpeed,
			m_smoothDampMaxSpeed
		);
		m_previousCameraOffsetPosition = m_cameraOffsetPosition;
	}

	public float MagnitudeFromAngle(float angle)
	{
		float absCosAngle = Mathf.Abs(Mathf.Cos(angle));
		float absSinAngle = Mathf.Abs(Mathf.Sin(angle));

		float magnitude = 0f;

		if (m_camera.pixelWidth / 2f * absSinAngle <= m_camera.pixelHeight / 2f * absCosAngle)
			magnitude = m_camera.pixelWidth / 2f / absCosAngle;
		else
			magnitude = m_camera.pixelHeight / 2f / absSinAngle;

		return magnitude;
	}

	#endregion

	#region Pause


	public void Pause()
	{
		isPaused = true;
	}

	public void Resume()
	{
		isPaused = false;
	}

	#endregion

	#endregion
}
