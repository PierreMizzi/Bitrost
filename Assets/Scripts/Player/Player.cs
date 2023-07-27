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

    private void Start()
    {
        m_levelChannel.player = gameObject;
        m_healthEntity = GetComponent<HealthEntity>();
        m_controller = GetComponent<PlayerController>();
        
        m_healthEntity.maxHealth = m_settings.maxHealth;
        SubscribeHealthEntity();
    }

    private void OnDestroy() {
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

	private void CallbackHealedHealth()
	{
	}

	private void CallbackLostHealth()
	{
	}

    private void CallbackNoHealth()
	{
        Debug.LogWarning("PLAYER HAS DIED !");
        m_controller.enabled = false;
	}



	#endregion

	#endregion
}
