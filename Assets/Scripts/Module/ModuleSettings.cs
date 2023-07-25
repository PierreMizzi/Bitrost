using UnityEngine;

[CreateAssetMenu(fileName = "ModuleSettings", menuName = "Bitrost/ModuleSettings", order = 0)]
public class ModuleSettings : ScriptableObject
{
    [Header("Module Manager")]
    public int startingModuleCount = 1;

    [Header("Module")]
    [Header("Energy")]
    public int storedEnergyCapacity = 5;

	public float extractionDuration = 3;

	public Bullet bulletPrefab = null;

	public float bulletSpeed;
}
