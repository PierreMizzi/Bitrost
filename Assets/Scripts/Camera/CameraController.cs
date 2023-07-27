using CodesmithWorkshop.Useful;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Transform m_target = null;

    [SerializeField]
    private float m_distance = 4f;

        [SerializeField]
    private InputActionReference m_mousePositionActionReference = null;

    private Vector3 m_mousePositionValue;

    private Camera m_camera = null;

    private Vector3 mousePositionScreenCenter
    {
        get {
            return new Vector3(m_mousePositionValue.x - m_camera.pixelWidth / 2f, m_mousePositionValue.y - m_camera.pixelHeight /2f);
        }
    }

    [SerializeField] private Vector2 m_debugCenterScreen;

    private float m_distanceFromCenter;

    [SerializeField] private float m_minRadius = 1f;
    [SerializeField] private float m_maxRadius = 2f;

    [SerializeField] private float m_offsetMagnitude = 1f;

    private float m_offset;

    private Vector3 m_cameraPosition;

    #endregion

    #region Methods

    private void Awake() {
        m_camera = Camera.main;
    }

    private void Update()
    {
        m_mousePositionValue = m_mousePositionActionReference.action.ReadValue<Vector2>();
        m_debugCenterScreen = mousePositionScreenCenter;
    }

    private void LateUpdate()
    {
        ManageCameraPosition();
        // transform.position = new Vector3(m_target.position.x, m_target.position.y, -m_distance);
    }

    private void ManageCameraPosition()
    {
        m_distanceFromCenter = (Vector3.zero - mousePositionScreenCenter).magnitude;
        m_offset = UtilsClass.Remap(m_distanceFromCenter, m_minRadius, m_maxRadius, 0f, 1f);
        m_offset = Mathf.Clamp01(m_offset);

        m_cameraPosition = m_target.position;
        m_cameraPosition.z = -1;


        m_cameraPosition += mousePositionScreenCenter.normalized * m_offset * m_offsetMagnitude; 
        transform.position = m_cameraPosition;
    }


    #endregion
}
