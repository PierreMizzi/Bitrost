using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Transform m_target = null;

    [SerializeField]
    private float m_distance = 4f;

    #endregion

    #region Methods

    private void LateUpdate()
    {
        transform.position = new Vector3(m_target.position.x, m_target.position.y, -m_distance);
    }

    #endregion
}
