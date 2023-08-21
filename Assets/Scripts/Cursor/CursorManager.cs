using System;
using System.Collections.Generic;
using Bitfrost.Application;
using UnityEngine;

namespace Bitfrost.Gameplay
{


    public delegate void CursorDelegate(CursorType type);

    public class CursorManager : MonoBehaviour
    {

        [SerializeField]
        private ApplicationChannel m_appChannel;

        [SerializeField]
        private LevelChannel m_levelChannel;

        [SerializeField] private List<CursorConfig> m_configs = new List<CursorConfig>();

        private Dictionary<CursorType, CursorConfig> m_typeToConfig = new Dictionary<CursorType, CursorConfig>();

        private void Start()
        {
            foreach (CursorConfig config in m_configs)
            {
                if (!m_typeToConfig.ContainsKey(config.type))
                    m_typeToConfig.Add(config.type, config);
            }

            if (m_appChannel != null)
                m_appChannel.onSetCursor += SetCursor;

            if (m_levelChannel != null)
            {
                m_levelChannel.onDefeatPanel += CallbackNormalCursor;
                m_levelChannel.onVictoryPanel += CallbackNormalCursor;

                m_levelChannel.onPauseGame += Pause;
            }
        }

        private void OnDestroy()
        {
            if (m_appChannel != null)
                m_appChannel.onSetCursor -= SetCursor;

            if (m_levelChannel != null)
            {
                m_levelChannel.onDefeatPanel -= CallbackNormalCursor;
                m_levelChannel.onVictoryPanel -= CallbackNormalCursor;

                m_levelChannel.onPauseGame -= Pause;
            }
        }

        private void SetCursor(CursorType type)
        {
            if (m_typeToConfig.ContainsKey(type))
            {
                CursorConfig config = m_typeToConfig[type];
                Cursor.SetCursor(config.texture, config.hotspot, config.mode);
            }
        }

        private void CallbackNormalCursor(GameOverData data)
        {
            SetCursor(CursorType.Normal);
        }

        private void Pause()
        {
            SetCursor(CursorType.Normal);
        }

    }
}