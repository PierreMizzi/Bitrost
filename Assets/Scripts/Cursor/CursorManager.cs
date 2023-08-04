using UnityEngine;

public delegate void CursorDelegate(CursorType type);

public class CursorManager : MonoBehaviour
{

    [SerializeField] private ApplicationChannel m_appChannel = null;

    [SerializeField]
    private Texture2D m_normalCursor = null;

    [SerializeField]
    private Texture2D m_attackCursor = null;

    private void Start()
    {
        CallbackSetCursor(CursorType.Normal);

        if (m_appChannel != null)
            m_appChannel.onSetCursor += CallbackSetCursor;

    }

    private void OnDestroy()
    {
        if (m_appChannel != null)
            m_appChannel.onSetCursor -= CallbackSetCursor;
    }

    private void CallbackSetCursor(CursorType type)
    {
        switch (type)
        {
            case CursorType.Normal:
                Cursor.SetCursor(m_normalCursor, Vector3.zero, CursorMode.Auto);
                break;
            case CursorType.Attack:
                Cursor.SetCursor(m_attackCursor, Vector3.zero, CursorMode.Auto);
                break;
            default:
                break;
        }

    }


}
