using UnityEngine;

public abstract class ABulletTrackNode : MonoBehaviour
{
    [SerializeField]
    protected float m_linkVisualRotationSpeed = 2f;

    [SerializeField]
    protected GameObject m_linkRadiusVisual = null;

    public ABulletTrackNode nextNode { get; set; }

    public bool isLinkable
    {
        get { return nextNode == null; }
    }

    public virtual void Update()
    {
        if (isLinkable)
        {
            m_linkRadiusVisual.transform.rotation *= Quaternion.Euler(
                Vector3.forward * Time.deltaTime * m_linkVisualRotationSpeed
            );
        }
    }

    public void RefreshState()
    {
        m_linkRadiusVisual.SetActive(isLinkable);
    }
}
