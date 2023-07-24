using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;

public class BulletTrack : MonoBehaviour
{
	#region Fields

    private BulletTrackManager m_manager = null;

    #region Track

    [Header("Track")]
    [SerializeField]
    private LineRenderer m_lineRenderer = null;

    public Spline spline = null;

    private List<ABulletTrackNode> m_nodes = new List<ABulletTrackNode>();

    public ExtractorNode extractor
    {
        get
        {
            if (m_nodes.Count > 0)
                return m_nodes[0] as ExtractorNode;
            else
                return null;
        }
    }

    public ABulletTrackNode lastNode
    {
        get { return m_nodes[m_nodes.Count - 1]; }
    }

    #endregion


    #region Bullets

    [Header("Bullets")]
    [SerializeField]
    private PlayerBullet m_bulletPrefab = null;

    private List<PlayerBullet> m_bullets = new List<PlayerBullet>();

    [SerializeField]
    private float m_bulletSpeed = 1f;
    public float bulletSpeed
    {
        get { return m_bulletSpeed; }
    }

    #endregion

	#endregion

	#region Methods

    public void Initialize(BulletTrackManager manager, CrystalShard crystal)
    {
        m_manager = manager;

        ExtractorNode extractor = Instantiate(
            m_manager.extractorPrefab,
            crystal.transform.position,
            Quaternion.identity,
            m_manager.nodeContainer
        );
        extractor.Initialize(crystal);

        m_nodes.Add(extractor);
        RebuildLinking();
    }

    public void Clear()
    {
        foreach (ABulletTrackNode node in m_nodes)
            Destroy(node.gameObject);

        m_nodes.Clear();
    }

	#region Nodes

    public void AddNode(TurretNode turret)
    {
        if (lastNode.GetType() == typeof(ExtractorNode))
        {
            lastNode.nextNode = turret;
            m_nodes.Add(turret);
            RebuildLinking();
        }
        else if (lastNode.GetType() == typeof(TurretNode))
        {
            Vector3 lastPosition = lastNode.transform.position;
            // Delete Turret
            Destroy(lastNode.gameObject);
            m_nodes.Remove(lastNode);

            // Adds
            LinkNode link = Instantiate(
                m_manager.linkPrefab,
                lastPosition,
                Quaternion.identity,
                m_manager.nodeContainer
            );

            m_nodes.Add(link);
            link.nextNode = turret;

            m_nodes.Add(turret);

            RebuildLinking();
        }
    }

    private void RebuildLinking()
    {
        Vector3 localPosition = transform.InverseTransformPoint(lastNode.transform.position);

        m_lineRenderer.positionCount += 1;
        m_lineRenderer.SetPosition(m_nodes.Count - 1, localPosition);

        BezierKnot knot = new BezierKnot(
            lastNode.transform.position,
            lastNode.transform.position,
            lastNode.transform.position,
            Quaternion.Euler(270, 0, 0)
        );
        
        spline.Add(knot, TangentMode.AutoSmooth);
    }

	#endregion

    #region Fire

    public void Fire()
    {
        if (CanFire())
        {
            extractor.Extract();
            CreateBullet();
        }
    }

    public bool CanFire()
    {
        bool result = true;

        bool canExtract = extractor.CanExtract();
        result &= canExtract;
        if (!canExtract)
            Debug.LogWarning("CRYSTAL IS DEPLEATED");

        bool hasATurret = lastNode.GetType() == typeof(TurretNode);
        result &= hasATurret;
        if (!hasATurret)
            Debug.LogWarning("No turret");

        return result;
    }

    public void CreateBullet()
    {
        PlayerBullet bullet = Instantiate(
            m_bulletPrefab,
            extractor.transform.position,
            Quaternion.identity,
            m_manager.bulletContainer
        );

        m_bullets.Add(bullet);

        bullet.Initialize(this);
    }

    #endregion

	#endregion
}
