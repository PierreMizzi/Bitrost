using UnityEngine;
using UnityEngine.InputSystem;

namespace Bitfrost.Gameplay.Players
{

    public class PlayerController : MonoBehaviour, IPausable
    {
        #region Fields

        [SerializeField]
        private PlayerSettings m_settings = null;

        private Camera m_camera;

        #region Inputs

        [SerializeField]
        private InputActionReference m_locomotionActionReference = null;

        [SerializeField]
        private InputActionReference m_mousePositionActionReference = null;

        #endregion

        #region Locomotion

        [SerializeField]
        private Vector3 m_locomotionActionValue;
        private Vector3 m_nextPosition;

        [SerializeField]
        private Vector2 m_mousePositionActionValue;
        private Vector2 m_screenSpacePosition;
        private Vector3 m_orientation;

        public bool isPaused { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Monobehaviour

        private void Start()
        {
            m_camera = Camera.main;
        }

        private void Update()
        {
            if (!isPaused)
                ReadInputs();
        }

        private void FixedUpdate()
        {
            if (!isPaused)
                Move();
        }

        private void LateUpdate()
        {
            if (!isPaused)
                Rotate();
        }

        #endregion

        #region Locomotion

        private void Move()
        {
            m_nextPosition =
                transform.position + (m_locomotionActionValue * m_settings.speed * Time.deltaTime);

            if (LevelManager.IsInsideArena(m_nextPosition))
                transform.position = m_nextPosition;
        }

        private void Rotate()
        {
            m_screenSpacePosition = m_camera.WorldToScreenPoint(transform.position);

            m_orientation = (m_mousePositionActionValue - m_screenSpacePosition).normalized;
            transform.right = m_orientation;
        }

        #endregion

        private void ReadInputs()
        {
            m_locomotionActionValue = m_locomotionActionReference.action.ReadValue<Vector2>();
            m_mousePositionActionValue = m_mousePositionActionReference.action.ReadValue<Vector2>();
        }

        #region Pause

        #endregion

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        #endregion
    }
}
