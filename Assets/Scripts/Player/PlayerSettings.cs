using UnityEngine;

namespace Bitfrost.Gameplay.Players
{

    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Bitrost/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {

        [Header("Health")]

        public float maxHealth = 300;
        public float healedHealthPerStoredEnergy = 5;

        [Header("Controller")]
        public float speed;



    }

}