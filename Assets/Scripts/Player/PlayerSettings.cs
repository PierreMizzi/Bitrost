using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Bitrost/PlayerSettings", order = 0)]
public class PlayerSettings : ScriptableObject
{
    [Header("Controller")]
    public float speed;

}
