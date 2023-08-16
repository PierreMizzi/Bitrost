namespace Bitfrost.Application.SolarSystem
{
	using System.Collections.Generic;
	using PierreMizzi.Rendering;
	using PierreMizzi.Useful;
	using UnityEngine;

	[ExecuteInEditMode]
	public class SpaceObject : MonoBehaviour
	{

		#region Fields 

		[Header("Settings")]

		[SerializeField]
		private Transform m_orbitCenter = null;

		[SerializeField]
		private float m_orbitRadius;

		[SerializeField]
		private float m_speed;

		private Vector3 m_orbitPosition;

		private Quaternion m_frameRotation;

		[Header("Rendering")]

		[SerializeField]
		private SpriteRenderer m_objectSprite;

		[Header("Orbit")]
		[SerializeField]
		private SpriteRenderer m_orbitSprite;

		private MaterialPropertyBlockModifier m_orbitPropertyBlock;

		private const string k_radiusProperty = "_Radius";

		[Header("System")]
		[SerializeField]
		private bool m_isSystemRoot;

		[SerializeField] private List<SpaceObject> m_children = new List<SpaceObject>();

		private float orbitRadiusShader
		{
			get
			{
				return m_orbitRadius * 0.9f / 13.5f;
			}
		}

		#endregion

		#region Methods 

		private void Awake()
		{
			InitializeRotation();
			InitializeOrbit();
		}

		private void OnEnable()
		{
			InitializeOrbit();
		}

		private void OnValidate()
		{
			if (m_orbitCenter == null)
				return;

			m_orbitSprite.transform.position = m_orbitCenter.position;
			m_orbitPosition = new Vector2(m_orbitRadius, 0);
			transform.position = m_orbitCenter.position + m_orbitPosition;

			if (m_orbitPropertyBlock != null)
				m_orbitPropertyBlock.SetProperty(k_radiusProperty, orbitRadiusShader);
		}

		protected void Update()
		{
			if (Application.isPlaying && m_isSystemRoot)
				UpdateOrbit();
		}

		protected void InitializeRotation()
		{
			Quaternion randomRotation = UtilsClass.RandomRotation2D();
			m_orbitPosition = new Vector2(m_orbitRadius, 0);
			m_orbitPosition = randomRotation * m_orbitPosition;

			m_orbitSprite.transform.rotation = randomRotation;
		}

		protected void UpdateOrbit()
		{
			m_frameRotation = Quaternion.Euler(0f, 0f, Time.deltaTime * m_speed);
			m_orbitPosition = m_frameRotation * m_orbitPosition;

			m_orbitSprite.transform.position = m_orbitCenter.position;
			m_orbitSprite.transform.rotation *= m_frameRotation;
			transform.position = m_orbitCenter.position + m_orbitPosition;

			foreach (SpaceObject child in m_children)
				child.UpdateOrbit();
		}


		protected void InitializeOrbit()
		{
			m_orbitPropertyBlock = m_orbitSprite.GetComponent<MaterialPropertyBlockModifier>();
			m_orbitPropertyBlock.SetProperty(k_radiusProperty, orbitRadiusShader);
		}


		#endregion


	}
}