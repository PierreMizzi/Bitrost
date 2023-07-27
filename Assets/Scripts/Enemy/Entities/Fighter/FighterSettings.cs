using UnityEngine;

[CreateAssetMenu(
    fileName = "FighterSettings",
    menuName = "Bitrost/Enemies/FighterSettings",
    order = 0
)]
public class FighterSettings : EnemySettings { 

	[Header("Movement")]

	public float radiusAroundPlayer;

	[Header("Attack")]
	public Bullet bulletPrefab;

	public float bulletSalvoCount;

	/// <summary> 
	///	Delay between each bullets in a salvo
	/// </summary>
	public float bulletSalvoFrequency;

}
