using UnityEngine;

public class Bullet : MonoBehaviour, IPausable
{
    #region Fields

    [Header("Bullet")]
    [SerializeField]
    protected BulletChannel m_bulletChannel = null;

    [SerializeField]
    protected BulletType m_type = BulletType.None;
    public BulletType type
    {
        get { return m_type; }
    }

    protected BulletManager m_manager = null;

    protected IBulletLauncher m_launcher;

    [SerializeField]
    protected float m_speed;

    #region Collision

    protected bool m_hasHit = false;

    [SerializeField]
    protected ContactFilter2D m_collisionFilter;

    #endregion

    public bool isPaused { get; set; }

    #endregion

    #region Methods

    protected virtual void OnEnable()
    {
        m_hasHit = false;
    }

    protected virtual void Update()
    {
        if (isPaused)
            return;

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

    public virtual void AssignLauncher(IBulletLauncher launcher)
    {
        m_launcher = launcher;
    }

    #region Collision

    public void RaycastForCollision() { }

    #endregion

    #region Pause

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    #endregion

    #endregion
}
