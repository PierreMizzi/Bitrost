using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Bitrost/PlayerSettings", order = 0)]
public class PlayerSettings : ScriptableObject
{

    [Header("Health")]

    public float maxHealth = 300;

    [Header("Controller")]
    public float speed;

}
