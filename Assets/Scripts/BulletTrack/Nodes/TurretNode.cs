using UnityEngine;

public class TurretNode : ABulletTrackNode
{
    private PlayerController m_player = null;

    private Vector3 m_aimDirection;

    public Vector3 aimDirection
    {
        get { return m_aimDirection; }
    }

    [SerializeField]
    private Transform m_sprite = null;

    protected void Awake()
    {
        m_player = FindObjectOfType<PlayerController>();
    }

    public override void Update()
    {
        base.Update();
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        m_aimDirection = (m_player.transform.position - transform.position).normalized;
        m_sprite.transform.up = m_aimDirection;
    }
}
