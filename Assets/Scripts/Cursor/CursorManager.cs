using System.Collections.Generic;
using Bitfrost.Application;
using UnityEngine;

namespace Bitfrost.Gameplay
{

    public delegate void CursorDelegate(CursorType type);

    /// <summary>
    /// Handles cursor's properties in-game when fighting or in menu
    /// </summary>
    public class CursorManager : MonoBehaviour
    {

        [SerializeField]
        private ApplicationChannel m_appChannel;

        [SerializeField]
        private List<CursorConfig> m_configs = new List<CursorConfig>();

        /// <summary>
        /// Associate cursor's configurations to one type of cursor
        /// </summary>
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

        }

        private void OnDestroy()
        {
            if (m_appChannel != null)
                m_appChannel.onSetCursor -= SetCursor;

        }

        /// <summary>
        /// Changes cursor propreties, like texture or hotspot
        /// </summary>
        /// <param name="type"></param>
        private void SetCursor(CursorType type)
        {
            if (m_typeToConfig.ContainsKey(type))
            {
                CursorConfig config = m_typeToConfig[type];
                Cursor.SetCursor(config.texture, config.hotspot, config.mode);
            }
        }

    }
}