using UnityEngine;

public class EnemySettings : ScriptableObject
{
    [Header("Health")]
    public float maxHealth = 100;

    [Header("Movement")]
    public float speed = 14f;
}
