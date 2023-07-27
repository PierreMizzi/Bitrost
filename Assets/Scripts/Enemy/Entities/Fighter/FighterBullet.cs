using CodesmithWorkshop.Useful;
using UnityEngine;

public class FighterBullet : Bullet {
	

	#region Fields 

	[SerializeField] private FighterSettings m_settings = null;

	#endregion 

	#region Methods 

	private void Start() {
		m_speed = m_settings.bulletSpeed;
	}

	    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_hasHit)
            return;

        if (UtilsClass.CheckLayer(m_collisionFilter.layerMask.value, other.gameObject.layer))
        {
         	if (other.gameObject.TryGetComponent(out HealthEntity healthEntity))
                HitPlayer(healthEntity);
        }
    }

	private void HitPlayer(HealthEntity healthEntity)
    {
        healthEntity.LoseHealth(m_settings.bulletDamage);
        Release();
        m_hasHit = true;
    }

	#endregion

}