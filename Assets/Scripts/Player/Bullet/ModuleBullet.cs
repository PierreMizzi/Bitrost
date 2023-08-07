using UnityEngine;
using PierreMizzi.Useful;

public class ModuleBullet : Bullet
{
    [Header("Module Bullet")]
    [SerializeField]
    private ModuleSettings m_settings = null;

    private CrystalShard m_originCrystal;

    public override void AssignLauncher(IBulletLauncher launcher)
    {
        base.AssignLauncher(launcher);
        Module module = m_launcher.gameObject.GetComponent<Module>();
        m_originCrystal = module.crystal;
        m_speed = m_settings.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_hasHit)
            return;

        if (UtilsClass.CheckLayer(m_collisionFilter.layerMask.value, other.gameObject.layer))
        {
            if (other.gameObject.TryGetComponent(out CrystalShard crystal))
            {
                if (crystal == m_originCrystal)
                    return;

                HitCrystal(crystal);
            }
            else if (other.gameObject.TryGetComponent(out HealthEntity healthEntity))
                HitHealthEntity(healthEntity);
        }
    }

    private void HitCrystal(CrystalShard crystal)
    {
        crystal.DecrementEnergy();
        Release();
        m_hasHit = true;
    }

    private void HitHealthEntity(HealthEntity healthEntity)
    {
        healthEntity.LoseHealth(m_settings.bulletDamage);
        Release();
        m_hasHit = true;
    }
}
