using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
#region Fields

    [SerializeField]
    private PlayerControllerSettings m_settings = null;

    private Camera m_camera;

	#region Inputs

    [SerializeField]
    private InputActionReference m_locomotionActionReference = null;

    [SerializeField]
    private InputActionReference m_mousePositionActionReference = null;

    // [SerializeField]
    // private InputActionReference m_fireActionReference = null;

    // [SerializeField]
    // private InputActionReference m_dropNodeActionReference = null;

    // [SerializeField]
    // private InputActionReference m_retrieveNodeActionReference = null;

	#endregion

	#region Locomotion

    [SerializeField]
    private Vector3 m_locomotionActionValue;
    private Vector3 m_nextPosition;

    [SerializeField]
    private Vector2 m_mousePositionActionValue;
    private Vector2 m_screenSpacePosition;
    private Vector3 m_orientation;

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
        ReadInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Rotate();
    }

    private void OnDestroy() { }

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

	#region Inputs

    private void ReadInputs()
    {
        m_locomotionActionValue = m_locomotionActionReference.action.ReadValue<Vector2>();
        m_mousePositionActionValue = m_mousePositionActionReference.action.ReadValue<Vector2>();
    }

    public void SubscribeInputs()
    {
        if (m_locomotionActionReference != null)
            m_locomotionActionReference.action.performed += CallbackLocomotionAction;

        if (m_mousePositionActionReference != null)
            m_mousePositionActionReference.action.performed += CallbackLocomotionAction;

        // if (m_fireActionReference != null)
        //     m_fireActionReference.action.performed += CallbackLocomotionAction;

        // if (m_dropNodeActionReference != null)
        //     m_dropNodeActionReference.action.performed += CallbackLocomotionAction;

        // if (m_retrieveNodeActionReference != null)
        //     m_retrieveNodeActionReference.action.performed += CallbackLocomotionAction;
    }

    public void UnsubscribeInputs()
    {
        if (m_locomotionActionReference != null)
            m_locomotionActionReference.action.performed -= CallbackLocomotionAction;

        if (m_mousePositionActionReference != null)
            m_mousePositionActionReference.action.performed -= CallbackMousePositionAction;
    }

    private void CallbackLocomotionAction(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void CallbackMousePositionAction(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

	#endregion

	#endregion
}
