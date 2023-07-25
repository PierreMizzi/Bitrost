using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected BulletChannel m_bulletChannel = null;

    [SerializeField]
    protected BulletType m_type = BulletType.None;
    public BulletType type
    {
        get { return m_type; }
    }

    protected BulletManager m_manager = null;

    public IBulletLauncher launcher;

    [SerializeField]
    protected float m_speed;

    #region Collision

    protected bool m_hasHit = false;

    [SerializeField]
    protected ContactFilter2D m_collisionFilter;

    #endregion

    #endregion

    #region Methods

    protected virtual void OnEnable()
    {
        m_hasHit = false;
    }

    protected virtual void Update()
    {
        transform.position += transform.up * m_speed * Time.deltaTime;

        CheckInsideArena();
    }

    protected void CheckInsideArena()
    {
        if (!LevelManager.IsInsideArena(transform.position))
            Release();
    }

    protected virtual void Release()
    {
        m_bulletChannel.onReleaseBullet.Invoke(this);
    }

    #region Collision

    public void RaycastForCollision() { }

    #endregion

    #endregion
}
