using UnityEngine;
using DG.Tweening;

public class Sandbox : MonoBehaviour
{
	[SerializeField] private Vector3 m_rotation;
	[SerializeField] private Transform m_transform = null;
	[SerializeField] private float m_duration = 2f;


	[ContextMenu("Test Vector")]
	public void TestVector()
	{
		transform.DORotate(m_rotation, m_duration);
	}

	[ContextMenu("Test Transform")]
	public void TestTransform()
	{
		Vector3 direction = new Vector3(0f, 0f, Vector3.Angle(transform.up, (m_transform.position - transform.position).normalized));
		transform.DORotate(direction, m_duration);
	}
}