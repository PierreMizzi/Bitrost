using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float m_speed;

    public void Initialize(float speed)
    {
        m_speed = speed;
    }

    private void Update()
    {
        transform.position += transform.up * m_speed * Time.deltaTime;

        CheckInsideArena();
    }

    private void CheckInsideArena()
    {
        if (!LevelManager.IsInsideArena(transform.position))
            Destroy(gameObject);
    }
}
