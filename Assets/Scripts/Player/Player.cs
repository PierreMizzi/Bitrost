using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(HealthEntity))]
public class Player : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private LevelChannel m_levelChannel = null;

    private HealthEntity m_healthEntity = null;

    [SerializeField]
    private PlayerSettings m_settings = null;

    private PlayerController m_controller = null;

    #endregion

    #region Methods

    private void Awake()
    {
        m_levelChannel.player = gameObject;

        m_controller = GetComponent<PlayerController>();

        m_healthEntity = GetComponent<HealthEntity>();
        m_healthEntity.Initialize(m_settings.maxHealth);
    }

    private void Start()
    {
        SubscribeHealthEntity();
    }

    private void OnDestroy()
    {
        UnsubscribeHealthEntity();
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

    #endregion
}
