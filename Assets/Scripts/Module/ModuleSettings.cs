using UnityEngine;

[CreateAssetMenu(fileName = "ModuleSettings", menuName = "Bitrost/ModuleSettings", order = 0)]
public class ModuleSettings : ScriptableObject
{
    [Header("Module Manager")]
    public int startingModuleCount = 1;

    [Header("Module")]
    [Header("Energy")]
    public int maxStoredEnergy = 5;
    public float productionDuration = 3;
    public int productionRatio = 2;

    [Header("Bullet")]
    public Bullet bulletPrefab = null;
    public float bulletSpeed;
    public float bulletDamage = 50;
}
