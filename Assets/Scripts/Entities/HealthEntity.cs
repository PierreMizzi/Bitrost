using UnityEngine;
using System;

public class HealthEntity : MonoBehaviour
{
	#region Fields

    public float currentHealth { get; private set; }
    public float maxHealth { get; set; }

	public Action onLostHealth = null;
	public Action onHealedHealth = null;
    public Action onNoHealth = null;

	#endregion

	#region Methods

    private void Awake()
    {
        onLostHealth = () => { };
        onHealedHealth = () => { };
        onNoHealth = () => { };
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }

    public void LoseHealth(float lost)
    {
        currentHealth -= lost;
		onLostHealth.Invoke();

        if (currentHealth < 0)
        {
            currentHealth = 0;
            onNoHealth.Invoke();
        }
    }

    public void HealHealth(float healed)
    {
        currentHealth += healed;
		onHealedHealth.Invoke();

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

	#endregion
}
