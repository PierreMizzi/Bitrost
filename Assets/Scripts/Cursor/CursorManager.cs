using System;
using UnityEngine;

public delegate void CursorDelegate(CursorType type);

public class CursorManager : MonoBehaviour
{

    [SerializeField]
    private ApplicationChannel m_appChannel;

    [SerializeField]
    private LevelChannel m_levelChannel;

    [SerializeField]
    private Texture2D m_normalCursor;

    [SerializeField]
    private Texture2D m_attackCursor;

    private void Start()
    {
        SetCursor(CursorType.Attack);

        if (m_appChannel != null)
            m_appChannel.onSetCursor += SetCursor;

        if (m_levelChannel != null)
        {
            m_levelChannel.onGameOverPanel += CallbackGameOverPanel;
            m_levelChannel.onReset += CallbackReset;

            m_levelChannel.onPauseGame += Pause;
            m_levelChannel.onResumeGame += Resume;
        }
    }

    private void OnDestroy()
    {
        if (m_appChannel != null)
            m_appChannel.onSetCursor -= SetCursor;

        if (m_levelChannel != null)
        {
            m_levelChannel.onGameOverPanel -= CallbackGameOverPanel;
            m_levelChannel.onReset -= CallbackReset;

            m_levelChannel.onPauseGame -= Pause;
            m_levelChannel.onResumeGame -= Resume;
        }
    }

    private void SetCursor(CursorType type)
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

    private void CallbackGameOverPanel(GameOverData data)
    {
        SetCursor(CursorType.Normal);
    }

    private void CallbackReset()
    {
        SetCursor(CursorType.Attack);
    }

    private void Pause()
    {
        SetCursor(CursorType.Normal);
    }

    private void Resume()
    {
        SetCursor(CursorType.Attack);
    }

}