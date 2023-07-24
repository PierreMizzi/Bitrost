using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTrackManager : MonoBehaviour
{
	#region Fields

    private Camera m_camera;

    [SerializeField]
    private ContactFilter2D m_crystalShardFilter;

    [SerializeField]
    private InputActionReference m_mousePositionActionReference = null;

    [SerializeField]
    private InputActionReference m_dropNodeActionReference = null;

    [SerializeField]
    private InputActionReference m_retrieveNodeActionReference = null;

    [Header("Bullet Track")]
    [SerializeField]
    private BulletTrack m_bulletTrackPrefab = null;

    private List<BulletTrack> m_bulletTracks = new List<BulletTrack>();

    [SerializeField]
    private Transform m_trackContainer = null;

    [Header("Bullet Track Nodes")]
    [SerializeField]
    private int m_amountAvailableNodes = 4;

    private int m_remainingAvailableNodes = 0;

    [SerializeField]
    private int m_linkingRadius = 5;

    public int linkingRadius
    {
        get { return m_linkingRadius; }
    }

    [SerializeField]
    private Transform m_nodeContainer = null;

    [SerializeField]
    private ExtractorNode m_extractorPrefab = null;

    [SerializeField]
    private LinkNode m_linkPrefab = null;

    [SerializeField]
    private TurretNode m_turretPrefab = null;

    public Transform nodeContainer
    {
        get { return m_nodeContainer; }
    }
    public ExtractorNode extractorPrefab
    {
        get { return m_extractorPrefab; }
    }
    public LinkNode linkPrefab
    {
        get { return m_linkPrefab; }
    }

	#endregion

    #region Bullets

    [Header("Bullets")]
    [SerializeField]
    private Transform m_bulletContainer = null;
    public Transform bulletContainer
    {
        get { return m_bulletContainer; }
    }

    #endregion

    #region Fire

    [Header("Fire")]
    [SerializeField]
    private InputActionReference m_fireActionReference = null;

    #endregion

	#region Methods

	#region MonoBehaviour

    private void OnEnable()
    {
        m_camera = Camera.main;
    }

    private void Start()
    {
        if (m_dropNodeActionReference != null)
            m_dropNodeActionReference.action.performed += CallbackDropNodeAction;

        if (m_retrieveNodeActionReference != null)
            m_retrieveNodeActionReference.action.performed += CallbackRetrieveNodeAction;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed += CallbackFireAction;

        m_remainingAvailableNodes = m_amountAvailableNodes;
    }

    private void Update() { }

    private void OnDestroy()
    {
        if (m_dropNodeActionReference != null)
            m_dropNodeActionReference.action.performed -= CallbackDropNodeAction;

        if (m_retrieveNodeActionReference != null)
            m_retrieveNodeActionReference.action.performed -= CallbackRetrieveNodeAction;

        if (m_fireActionReference != null)
            m_fireActionReference.action.performed -= CallbackFireAction;
    }

	#endregion

	#region Build

    private void CallbackDropNodeAction(InputAction.CallbackContext context)
    {
        Debug.Log("Click");
        Vector3 mouseScreenPosition = m_mousePositionActionReference.action.ReadValue<Vector2>();
        Vector3 raycastOrigin = m_camera.ScreenToWorldPoint(mouseScreenPosition);
        raycastOrigin.z = m_camera.transform.position.z;

        List<RaycastHit2D> results = new List<RaycastHit2D>();

        if (Physics2D.Raycast(raycastOrigin, Vector3.forward, m_crystalShardFilter, results) > 0)
        {
            if (results[0].transform.TryGetComponent<CrystalShard>(out CrystalShard crystal))
                CreateBulletTrack(crystal);
        }
        else
        {
            CreateNode(raycastOrigin);
            Debug.Log($"Click screen : {mouseScreenPosition} | click world: {raycastOrigin}");
        }
    }

	#endregion

	#region Retrieve

    private void CallbackRetrieveNodeAction(InputAction.CallbackContext context)
    {
        foreach (BulletTrack track in m_bulletTracks)
        {
            track.Clear();
            Destroy(track.gameObject);
        }
        m_bulletTracks.Clear();
        m_remainingAvailableNodes = m_amountAvailableNodes;
    }

	#endregion

    #region Fire

    private void CallbackFireAction(InputAction.CallbackContext context)
    {
        foreach (BulletTrack track in m_bulletTracks)
        {
            Debug.Log("FIRE !");
            track.Fire();
        }
    }

    #endregion

	#region Bullet Track Nodes

    private bool CanCreateExtractor(CrystalShard crystal)
    {
        bool result = true;

        if (m_remainingAvailableNodes < 1)
        {
            Debug.LogWarning("NO AVAILABLE NODE");
            result &= m_remainingAvailableNodes < 1;
        }
        else if (crystal.isExtracted)
        {
            Debug.LogWarning("CRYSTAL IS ALREADY EXTRACTED>");
            result &= crystal.isExtracted;
        }
        return result;
    }

    private void CreateBulletTrack(CrystalShard crystal)
    {
        if (CanCreateExtractor(crystal))
        {
            BulletTrack bulletTrack = Instantiate(
                m_bulletTrackPrefab,
                crystal.transform.position,
                Quaternion.identity,
                m_trackContainer
            );

            bulletTrack.Initialize(this, crystal);
            m_bulletTracks.Add(bulletTrack);
            m_remainingAvailableNodes--;
        }
    }

    private bool CanCreateNode(Vector3 worldPosition)
    {
        if (m_remainingAvailableNodes < 1)
        {
            Debug.LogWarning("NO AVAILABLE NODE");
            return false;
        }
        else
        {
            float distance;

            foreach (BulletTrack track in m_bulletTracks)
            {
                distance = (worldPosition - track.lastNode.transform.position).magnitude;

                if (distance < m_linkingRadius)
                    return true;
            }

            Debug.LogWarning("POSITION OUT OF RANGE");
            return false;
        }
    }

    private void CreateNode(Vector3 originRaycast)
    {
        Vector3 worldPosition = originRaycast;
        worldPosition.z = 0f;

        if (CanCreateNode(worldPosition))
        {
            TurretNode turretNode = Instantiate(
                m_turretPrefab,
                worldPosition,
                Quaternion.identity,
                m_nodeContainer
            );

            BulletTrack track = FindLinkingTrack(worldPosition);

            track.AddNode(turretNode);
            m_remainingAvailableNodes--;
        }
    }

    private BulletTrack FindLinkingTrack(Vector3 worldPosition)
    {
        BulletTrack linkingTrack = null;
        float shortestDistance = 9999f;
        float distance = 0f;

        foreach (BulletTrack track in m_bulletTracks)
        {
            distance = (worldPosition - track.lastNode.transform.position).magnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                linkingTrack = track;
            }
        }

        return linkingTrack;
    }

	#endregion

	#endregion
}
