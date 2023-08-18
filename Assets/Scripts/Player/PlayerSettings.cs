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
        public float speed = 80f;

        public float smoothTime = 0.5f;

        public float immediatePositionScale = 0.1f;



    }

}