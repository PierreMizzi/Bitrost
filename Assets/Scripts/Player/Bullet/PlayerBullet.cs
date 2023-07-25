using UnityEngine;
using CodesmithWorkshop.Useful;

public class PlayerBullet : Bullet
{
    public CrystalShard originCrystal;

    protected virtual void Start()
    {
        Module module = launcher.gameObject.GetComponent<Module>();
        originCrystal = module.crystal;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_hasHit)
            return;

        if (UtilsClass.CheckLayer(m_collisionFilter.layerMask.value, other.gameObject.layer))
        {
            if (other.gameObject.TryGetComponent(out CrystalShard crystal))
            {
                if (crystal == originCrystal)
                    return;

                crystal.DecrementEnergy();
                Release();
                m_hasHit = true;
            }
            else if (other.gameObject.TryGetComponent(out Enemy enemy)) { }
        }
    }
}
