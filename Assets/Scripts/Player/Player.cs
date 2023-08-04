using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(HealthEntity))]
public class Player : MonoBehaviour, IPausable
{
    #region Fields

    [SerializeField]
    private LevelChannel m_levelChannel = null;

    private HealthEntity m_healthEntity = null;

    public HealthEntity healthEntity { get { return m_healthEntity; } }

    public bool isPaused { get; set; }

    [SerializeField]
    private PlayerSettings m_settings = null;

    private PlayerController m_controller = null;

    #endregion

    #region Methods

    private void Awake()
    {
        m_levelChannel.player = this;

        m_controller = GetComponent<PlayerController>();
        m_healthEntity = GetComponent<HealthEntity>();
    }

    private void Start()
    {
        m_healthEntity.Initialize(m_settings.maxHealth);
        SubscribeHealthEntity();

        if (m_levelChannel != null)
        {
            m_levelChannel.onReset += CallbackReset;
            m_levelChannel.onPauseGame += Pause;
            m_levelChannel.onResumeGame += Resume;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeHealthEntity();

        if (m_levelChannel != null)
        {
            m_levelChannel.onReset -= CallbackReset;
            m_levelChannel.onPauseGame -= Pause;
            m_levelChannel.onResumeGame -= Resume;

        }
    }

    #region Health

    private void SubscribeHealthEntity()
    {
        m_healthEntity.onLostHealth += CallbackLostHealth;
        m_healthEntity.onHealedHealth += CallbackHealedHealth;
        m_healthEntity.onNoHealth += CallbackNoHealth;
    }

    private void UnsubscribeHealthEntity()
    {
        m_healthEntity.onLostHealth -= CallbackLostHealth;
        m_healthEntity.onHealedHealth -= CallbackHealedHealth;
        m_healthEntity.onNoHealth -= CallbackNoHealth;
    }

    private void CallbackHealedHealth() { }

    private void CallbackLostHealth() { }

    [ContextMenu("CallbackNoHealth")]
    private void CallbackNoHealth()
    {
        Debug.LogWarning("PLAYER HAS DIED !");
        m_levelChannel.onGameOver.Invoke();
        m_controller.enabled = false;
    }

    #endregion

    #region Reset - Restart

    public void CallbackReset()
    {
        m_healthEntity.Reset();
        transform.position = Vector3.zero;
        m_controller.enabled = true;
    }

    public void Pause()
    {
        m_controller.Pause();
    }

    public void Resume()
    {
        m_controller.Resume();
    }

    #endregion

    #endregion
}
