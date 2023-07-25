using UnityEngine;
using CodesmithWorkshop.Useful;

public class TestCollision : MonoBehaviour
{
    [SerializeField]
    protected ContactFilter2D m_collisionFilter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((m_collisionFilter.layerMask.value & 1 << other.gameObject.layer) != 0)
        {
            Debug.Log("Right collision !");
        }

        if (UtilsClass.CheckLayer(m_collisionFilter.layerMask.value, other.gameObject.layer))
        {
            Debug.Log("Right collision Utils !");
        }
    }
}
