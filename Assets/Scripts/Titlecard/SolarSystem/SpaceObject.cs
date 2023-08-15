namespace Bitfrost.Application.SolarSystem
{
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

		private float m_rotationAngle;

		[Header("Rendering")]

		[SerializeField]
		private SpriteRenderer m_objectSprite;

		[SerializeField]
		private ParticleSystem m_trail;

		[Header("Orbit")]

		[SerializeField]
		private SpriteRenderer m_orbitSprite;

		private MaterialPropertyBlockModifier m_orbitPropertyBlock;

		private const string k_radiusProperty = "_Radius";

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
			m_orbitSprite.transform.position = m_orbitCenter.position;
			m_orbitPosition = new Vector2(m_orbitRadius, 0);
			transform.position = m_orbitCenter.position + m_orbitPosition;

			if (m_orbitPropertyBlock != null)
				m_orbitPropertyBlock.SetProperty(k_radiusProperty, orbitRadiusShader);
		}

		protected void Update()
		{
			if (Application.isPlaying)
				UpdateRotation();
		}

		protected void LateUpdate()
		{
			if (Application.isPlaying)
			{
				m_orbitSprite.transform.position = m_orbitCenter.position;
				transform.position = m_orbitCenter.position + m_orbitPosition;
			}
		}

		protected void InitializeRotation()
		{
			Quaternion randomRotation = UtilsClass.RandomRotation2D();
			m_orbitPosition = new Vector2(m_orbitRadius, 0);
			m_orbitPosition = randomRotation * m_orbitPosition;
		}

		protected void UpdateRotation()
		{
			m_orbitPosition = Quaternion.Euler(0f, 0f, Time.deltaTime * m_speed) * m_orbitPosition;
		}


		protected void InitializeOrbit()
		{
			m_orbitPropertyBlock = m_orbitSprite.GetComponent<MaterialPropertyBlockModifier>();
			m_orbitPropertyBlock.SetProperty(k_radiusProperty, orbitRadiusShader);
		}


		#endregion


	}
}