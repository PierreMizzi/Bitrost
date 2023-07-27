using UnityEngine;

[CreateAssetMenu(fileName = "HarvesterSettings", menuName = "Bitrost/Enemies/HarvesterSettings", order = 0)]
public class HarvesterSettings : EnemySettings
{


    [Header("Search Crystal")]
    [SerializeField]
    private float rangeAroundPlayer = 10f;

	[HideInInspector]
    public float rangeAroundPlayerSqr;

    public float offsetFromShard = 0.5f;

    [Header("Attack")]
    public float attackDelay = 1f;
    public float attackSpeed = 2f;

    private void OnEnable()
    {
        rangeAroundPlayerSqr = Mathf.Pow(rangeAroundPlayer, 2f);
    }
}
