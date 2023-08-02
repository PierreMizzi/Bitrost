using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D m_normalCursor = null;

    [SerializeField]
    private Texture2D m_attackCursor = null;

    private void Start()
    {
        Cursor.SetCursor(m_attackCursor, Vector3.zero, CursorMode.Auto);
    }
}
