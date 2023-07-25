using UnityEngine;

public interface IBulletLauncher
{
    public GameObject gameObject { get; }

    public void Fire();

    public bool CanFire();
}
