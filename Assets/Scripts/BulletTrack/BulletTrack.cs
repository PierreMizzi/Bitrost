using UnityEngine;
using System.Collections.Generic;

public class BulletTrack : MonoBehaviour
{
	#region Fields

	[SerializeField]
    private LineRenderer m_lineRenderer = null;

    private BulletTrackManager m_manager = null;

    private CrystalShard m_crystalShard = null;

    private List<ABulletTrackNode> m_nodes = new List<ABulletTrackNode>();

    public ABulletTrackNode lastNode
    {
        get { return m_nodes[m_nodes.Count - 1]; }
    }

    private ExtractorNode m_extractor
    {
        get
        {
            if (m_nodes.Count > 1)
                return m_nodes[0] as ExtractorNode;
            else
            {
                return null;
            }
        }
    }

	#endregion

	#region Methods

    public void Initialize(BulletTrackManager manager, CrystalShard crystal)
    {
        m_manager = manager;
        m_crystalShard = crystal;

        ExtractorNode extractor = Instantiate(
            m_manager.extractorPrefab,
            m_crystalShard.transform.position,
            Quaternion.identity,
            m_manager.nodeContainer
        );

        m_nodes.Add(extractor);
        extractor.RefreshState();
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
        int count = m_nodes.Count;
        m_lineRenderer.positionCount = count;
        for (int i = 0; i < count; i++)
        {
            ABulletTrackNode node = m_nodes[i];
            node.RefreshState();
            m_lineRenderer.SetPosition(i, transform.InverseTransformPoint(node.transform.position));

            if (i < count - 1)
                node.nextNode = m_nodes[i + 1];
            else
                node = null;
        }
    }

	#endregion

	#endregion
}
