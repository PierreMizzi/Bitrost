using UnityEngine;

[CreateAssetMenu(
    fileName = "FighterSettings",
    menuName = "Bitrost/Enemies/FighterSettings",
    order = 0
)]
public class FighterSettings : EnemySettings
{
    [Header("Movement")]
    public float radiusAroundPlayer;

    [Header("Attack")]
    public Bullet bulletPrefab;

    public float bulletSpeed;

    public float bulletDamage;

    /// <summary> 
    /// Amount of bullets in a salvo
    /// </summary>
    public int bulletSalvoCount = 3;

    /// <summary>
    ///	Delay between each bullets in a salvo
    /// </summary>
    public float bulletSalvoRateOfFire = 0.25f;

    /// <summary> 
    /// Delay between two salvos
    /// </summary>
    public float delayBetweenSalvo = 4f;
}
