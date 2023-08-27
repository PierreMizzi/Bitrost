using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    [CreateAssetMenu(fileName = "HarvesterSettings", menuName = "Bitrost/Enemies/HarvesterSettings", order = 0)]
    public class HarvesterSettings : EnemySettings
    {


        [Header("Search Crystal")]
        [SerializeField]
        private float searchCrystalRange = 10f;

        public float searchCrystalRangeSqr { get; private set; }

        // public float offsetFromShard = 0.5f;

        [Header("Attack")]
        public float attackDelay = 1f;
        public float attackSpeed = 2f;

        private void OnEnable()
        {
            searchCrystalRangeSqr = Mathf.Pow(searchCrystalRange, 2f);
        }
    }
}
