using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerControllerSettings",
    menuName = "Bitrost/PlayerControllerSettings",
    order = 0
)]
public class PlayerControllerSettings : ScriptableObject { 

	[Header("Locomotion")]

	public float speed;

}
