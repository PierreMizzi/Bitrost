using System;
using System.Collections.Generic;
using Bitfrost.Gameplay.Energy;
using PierreMizzi.Useful;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Bitfrost.Gameplay.Turrets
{
    public class TurretManager : MonoBehaviour, IPausable
    {
        #region Fields

        [SerializeField]
        private LevelChannel m_levelChannel = null;

        public bool isPaused { get; set; }

        #region Inputs

        private Camera m_camera;

        [SerializeField]
        private TurretSettings m_settings = null;

        [Header("Inputs")]

        [SerializeField]
        private InputActionReference m_mousePositionInput = null;

        [SerializeField]
        private InputActionReference m_dropRetrieveTurretInput = null;

        [SerializeField]
        private InputActionReference m_fireInput = null;

        [SerializeField]
        private InputActionReference m_switchModeInput = null;

        #endregion

        #region Turret

        [Header("Turret")]
        [SerializeField]
        private Turret m_turretPrefab = null;

        [SerializeField]
        private Transform m_turretContainer = null;

        private List<Turret> m_turrets = new List<Turret>();

        private int m_remainingTurretCount = 0;

        private bool hasAvailableTurret
        {
            get { return m_remainingTurretCount > 0; }
        }

        #endregion

        #region Turret Views

        [Header("Turret Views")]
        [SerializeField]
        private UIDocument m_document = null;

        private const string k_turretViewVisualContainer = "module-container";

        public VisualElement m_turretVisualsContainer;

        [SerializeField]
        private VisualTreeAsset m_turretVisualTemplate;

        public List<TurretView> m_turretViews = new List<TurretView>();

        #endregion

        #region Turret targeter

        [Header("Turret Targeter")]
        [SerializeField]
        private TurretTargeter m_turretTargeter;

        [SerializeField]
        private ContactFilter2D m_targeterFilter;

        private ATarget m_currentTarget;

        private List<RaycastHit2D> m_potentialTargets = new List<RaycastHit2D>();

        #endregion

        #endregion

        #region Methods

        #region MonoBehaviour

        private void OnEnable()
        {
            m_camera = Camera.main;
        }

        private void Start()
        {
            m_remainingTurretCount = m_settings.startingTurretCount;
            SubscribeInputs();

            m_turretVisualsContainer = m_document.rootVisualElement.Q(k_turretViewVisualContainer);

            CreateTurret();

            if (m_levelChannel != null)
            {
                m_levelChannel.onReset += CallbackReset;
                m_levelChannel.onPauseGame += Pause;
                m_levelChannel.onResumeGame += Resume;
            }

        }



        public void Update()
        {
            if (!isPaused)
                ManagerCurrentTarget();
        }

        private void OnDestroy()
        {
            UnsubscribeInputs();

            if (m_levelChannel != null)
            {
                m_levelChannel.onReset -= CallbackReset;
                m_levelChannel.onPauseGame -= Pause;
                m_levelChannel.onResumeGame -= Resume;
            }
        }

        #endregion

        #region Inputs

        private void SubscribeInputs()
        {
            if (m_dropRetrieveTurretInput != null)
                m_dropRetrieveTurretInput.action.performed += CallbackDropRetrieveTurret;

            if (m_fireInput != null)
                m_fireInput.action.performed += CallbackFire;

            if (m_switchModeInput != null)
                m_switchModeInput.action.performed += CallbackSwitchMode;
        }

        private void UnsubscribeInputs()
        {
            if (m_dropRetrieveTurretInput != null)
                m_dropRetrieveTurretInput.action.performed -= CallbackDropRetrieveTurret;

            if (m_fireInput != null)
                m_fireInput.action.performed -= CallbackFire;

            if (m_switchModeInput != null)
                m_switchModeInput.action.performed -= CallbackSwitchMode;
        }

        private void CallbackDropRetrieveTurret(InputAction.CallbackContext context)
        {
            if (m_currentTarget == null)
                return;

            switch (m_currentTarget.type)
            {
                case TargetType.CrystalShard:
                    DropTurret();
                    break;
                case TargetType.Turret:
                    RetrieveTurret();
                    break;
                default:
                    break;
            }
        }

        private void CallbackFire(InputAction.CallbackContext context)
        {
            foreach (Turret turret in m_turrets)
                turret.Fire();
        }

        private void CallbackSwitchMode(InputAction.CallbackContext context)
        {
            if (m_currentTarget == null)
                return;

            switch (m_currentTarget.type)
            {
                case TargetType.Turret:
                    SwitchTurretMode();
                    break;
                case TargetType.CrystalShard:
                // TODO Redrop on asteroid
                default:
                    break;
            }
        }

        private void SwitchTurretMode()
        {
            Turret turret = ((TurretTarget)m_currentTarget).turret;

            turret.SwitchMode();
        }

        #endregion

        #region Turret

        public void CreateTurret()
        {
            // Instantiate Model
            Turret turret = Instantiate(m_turretPrefab, m_turretContainer);
            turret.Initialize(this);
            m_turrets.Add(turret);

            // Instiate View, link it to model
            TurretView turretView = new TurretView();
            VisualElement turretViewVisual = m_turretVisualTemplate.Instantiate();
            m_turretVisualsContainer.Add(turretViewVisual);
            turretView.Initialize(turret, turretViewVisual);
            m_turretViews.Add(turretView);
        }

        private void DropTurret()
        {
            CrystalShard crystal = ((CrystalShardTarget)m_currentTarget).crystal;

            if (!crystal.isOccupied)
            {
                if (hasAvailableTurret)
                {
                    Turret turret = GetInactiveTurret();

                    if (turret != null)
                        turret.Drop(crystal);
                }
                else
                { /* TODO : Redeploy Turret*/
                }
            }
        }

        private void RetrieveTurret()
        {
            Turret turret = ((TurretTarget)m_currentTarget).turret;

            m_levelChannel.onTurretRetrieved.Invoke(turret.storedEnergy);
            turret.Retrieve();
        }

        private Turret GetInactiveTurret()
        {
            int count = m_turrets.Count;
            for (int i = 0; i < count; i++)
            {
                if ((TurretStateType)m_turrets[i].currentState.type == TurretStateType.Inactive)
                    return m_turrets[i];
            }
            return null;
        }

        #endregion

        #region Turret Targeter

        private void ManagerCurrentTarget()
        {
            Vector3 mouseScreenPosition = m_mousePositionInput.action.ReadValue<Vector2>();
            Vector3 raycastOrigin = ScreenPositionToRaycastOrigin(mouseScreenPosition);

            if (Physics2D.Raycast(raycastOrigin, Vector3.forward, m_targeterFilter, m_potentialTargets) > 0)
            {
                ATarget target = FindFirst(m_potentialTargets, TargetType.Turret);
                if (target != null)
                {
                    ManageTurretTarget(target);
                    return;
                }

                target = FindFirst(m_potentialTargets, TargetType.CrystalShard);
                if (target != null)
                {
                    ManageCrystalTarget(target);
                    return;
                }
            }
            else
                ManageNoTarget();
        }

        private void ManageCrystalTarget(ATarget target)
        {
            m_turretTargeter.SetTarget(target);
            m_currentTarget = target;

            if (m_remainingTurretCount > 0)
            {
                Turret turret = GetInactiveTurret();
                if (turret != null)
                    turret.isDroppable = true;
            }
        }

        private void ManageTurretTarget(ATarget target)
        {
            m_turretTargeter.SetTarget(target);
            m_currentTarget = target;

            ((TurretTarget)m_currentTarget).turret.isTargeted = true;
        }

        private void ManageNoTarget()
        {
            if (m_currentTarget != null)
            {
                switch (m_currentTarget.type)
                {
                    case TargetType.CrystalShard:
                        UnsetCrystalTarget();
                        break;
                    case TargetType.Turret:
                        UnsetTurretTarget();
                        break;
                }
            }
        }

        private void UnsetCrystalTarget()
        {
            m_turretTargeter.UnsetTarget();
            m_currentTarget = null;

            if (m_remainingTurretCount > 0)
            {
                Turret turret = GetInactiveTurret();
                if (turret != null)
                    turret.isDroppable = false;
            }
        }

        private void UnsetTurretTarget()
        {
            ((TurretTarget)m_currentTarget).turret.isTargeted = false;

            m_turretTargeter.UnsetTarget();
            m_currentTarget = null;
        }

        public ATarget FindFirst(List<RaycastHit2D> results, TargetType type)
        {
            foreach (RaycastHit2D hit in results)
            {
                if (hit.collider.TryGetComponent(out ATarget target))
                {
                    if (target.type == type)
                        return target;
                }
            }
            return null;
        }

        public Vector3 ScreenPositionToRaycastOrigin(Vector2 screenPosition)
        {
            Vector3 raycastOrigin = m_camera.ScreenToWorldPoint(screenPosition);
            raycastOrigin.z = m_camera.transform.position.z;
            return raycastOrigin;
        }

        #endregion

        #region Reset

        public void CallbackReset()
        {
            UtilsClass.EmptyTransform(m_turretContainer);
            m_turrets.Clear();

            // Turrets View
            m_turretViews.Clear();
            m_turretVisualsContainer.Clear();

            // Turrets
            m_remainingTurretCount = m_settings.startingTurretCount;
            CreateTurret();

        }

        #endregion

        #region Pause

        public void Pause()
        {
            isPaused = true;
            UnsubscribeInputs();

            foreach (Turret turret in m_turrets)
                turret.Pause();
        }

        public void Resume()
        {
            isPaused = false;
            SubscribeInputs();

            foreach (Turret turret in m_turrets)
                turret.Resume();
        }

        #endregion

        #endregion
    }
}
