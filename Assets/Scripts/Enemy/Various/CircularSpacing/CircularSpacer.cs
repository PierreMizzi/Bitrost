using UnityEngine;
using System.Collections.Generic;
using PierreMizzi.Useful;

namespace Bitfrost.Gameplay
{

	[ExecuteInEditMode]
	public class CircularSpacer : MonoBehaviour
	{

		#region Fields 

		[Header("Settings")]
		[SerializeField, Range(0, 10)]
		private float m_radius;

		[SerializeField, Range(0, 20)]
		private int m_count;

		[SerializeField, Range(0f, 360f)]
		private float m_offsetAngle;

		public float radius { get { return m_radius; } set { m_radius = Mathf.Max(0, value); } }
		public int count { get { return m_count; } set { m_count = Mathf.Max(0, value); } }
		public float offsetAngle { get { return m_offsetAngle; } set { m_offsetAngle = Mathf.Clamp(value, 0f, 360f); } }


		List<CircularSpacerSpot> m_spots = new List<CircularSpacerSpot>();

		public bool hasAvailableSpot
		{
			get
			{
				foreach (CircularSpacerSpot spot in m_spots)
				{
					if (spot.isAvailable)
						return true;
				}
				return false;
			}
		}

		[Header("Previzualization")]
		[SerializeField] private bool m_usePrevizualization = true;
		[SerializeField] private Sprite m_sprite;
		[SerializeField] private Color m_availableColor;
		[SerializeField] private Color m_unavailableColor;
		[SerializeField] private float m_spriteScale;
		[SerializeField] private bool m_lookAtOrigin;

		private List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();

		#endregion

		#region Methods 

		private void OnEnable()
		{
			Initialize();

#if !UNITY_EDITOR
			m_usePrevizualization = false;
#endif

			if (m_usePrevizualization)
				InitializePrevizualization();
		}

		private void OnValidate()
		{
			Initialize();
		}

		public void Initialize()
		{
			m_spots.Clear();

			float anglePerEnemy = 360f / m_count;
			float angle;
			Vector3 direction = Vector3.zero;
			Vector3 localPosition;
			CircularSpacerSpot spot;

			SpriteRenderer spriteRenderer;

			for (int i = 0; i < m_count; i++)
			{
				angle = Mathf.Deg2Rad * (m_offsetAngle + anglePerEnemy * i);
				direction.x = Mathf.Cos(angle);
				direction.y = Mathf.Sin(angle);
				localPosition = direction * m_radius;
				spot = new CircularSpacerSpot(transform, localPosition);
				m_spots.Add(spot);

				if (m_usePrevizualization && m_sprites.Count == m_spots.Count)
				{
					spriteRenderer = m_sprites[i];
					spriteRenderer.color = spot.isAvailable ? m_availableColor : m_unavailableColor;
				}
			}
		}

		public CircularSpacerSpot GetSpot(Vector3 closestPosition)
		{
			Vector3 directionToReference = (closestPosition - transform.position).normalized;
			float currentDot;
			float closestDot = 0;
			CircularSpacerSpot closestSpot = null;

			foreach (CircularSpacerSpot spot in m_spots)
			{
				currentDot = Vector3.Dot(directionToReference, spot.direction);
				if (spot.isAvailable && currentDot > closestDot)
				{
					closestDot = currentDot;
					closestSpot = spot;
				}
			}
			return closestSpot;
		}

		public CircularSpacerSpot GetSpotReworked(Vector3 closestPosition)
		{
			Vector3 directionToReference = (closestPosition - transform.position).normalized;
			float currentAngle;
			float smallestAngle = 180;
			CircularSpacerSpot closestSpot = null;

			foreach (CircularSpacerSpot spot in m_spots)
			{
				currentAngle = Vector3.Angle(directionToReference, spot.direction);
				if (spot.isAvailable && currentAngle < smallestAngle)
				{
					smallestAngle = currentAngle;
					closestSpot = spot;
				}
			}
			return closestSpot;
		}

		[ContextMenu("Initialize Previzualization")]
		private void InitializePrevizualization()
		{
			ClearPrevizualization();

			CircularSpacerSpot spot;
			for (int i = 0; i < m_spots.Count; i++)
			{
				spot = m_spots[i];

				GameObject @object = new GameObject($"Previz_{i}");
				@object.transform.parent = transform;
				@object.transform.position = spot.position;
				@object.transform.up = m_lookAtOrigin ? -spot.direction : Vector3.up;
				@object.transform.localScale = new Vector3(m_spriteScale, m_spriteScale, m_spriteScale);

				SpriteRenderer spriteRenderer = @object.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = m_sprite;
				spriteRenderer.color = spot.isAvailable ? m_availableColor : m_unavailableColor;

				m_sprites.Add(spriteRenderer);
			}
		}

		[ContextMenu("Clear Previzualization")]
		private void ClearPrevizualization()
		{
			UtilsClass.EmptyTransform(transform);
			m_sprites.Clear();
		}

		#endregion

	}
}