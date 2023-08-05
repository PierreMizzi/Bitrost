using UnityEngine;

[CreateAssetMenu(fileName = "ScoutSettings", menuName = "Bitrost/Enemies/Scout Settings", order = 0)]
public class ScoutSettings : EnemySettings
{

	[Header("Search Crystal")]
	public float radiusAroundPlayer = 10f;
	public float angleAroundPlayer;

	public float minSpeedTrackPlayer;
	public float maxSpeedTrackPlayer;

	public float delayBetweenBullet = 4f;

	public Bullet bulletPrefab;

}