using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private BulletType m_type = BulletType.None;
    public BulletType type
    {
        get { return m_type; }
    }

    private BulletManager m_manager = null;

    [SerializeField]
    private float m_speed;

    [SerializeField]
    private ContactFilter2D m_crystalShardFilter;

    [SerializeField]
    private BulletChannel m_bulletChannel = null;

    #endregion

    #region Methods

    private void Update()
    {
        transform.position += transform.up * m_speed * Time.deltaTime;

        CheckInsideArena();
    }

    private void CheckInsideArena()
    {
        if (!LevelManager.IsInsideArena(transform.position))
            m_bulletChannel.onReleaseBullet.Invoke(this);
    }

    #endregion
}
