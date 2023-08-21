using UnityEngine;
using System;
using System.Collections.Generic;
using Bitfrost.Gameplay.Enemies;

namespace Bitfrost.Gameplay
{

	/*

    RadialSpacer.cs

		Space enemies evenly around a target

		Config 
			Data

		RadialSpacingSpot
			Vector3 worldPosition;
			bool isAvailable;

	*/

	[Serializable]
	public class EnemySpacingConfig
	{
		public EnemyType enemyType;
		public float radius;
		public float count;
		public float angle;

		public bool drawGizmos;
		public Color gizmosColor;
		public float gizmosRadius;
	}

	public class CicularSpacingSpot
	{
		public Vector3 origin;
		public Vector3 direction;
		public Vector3 localPosition;
		public bool isAvailable = true;

		public Vector3 position => origin + localPosition;

		public CicularSpacingSpot(Vector3 origin, Vector3 localPosition)
		{
			this.origin = origin;
			this.localPosition = localPosition;
			this.direction = localPosition.normalized;
		}
	}

	[ExecuteInEditMode]
	public class EnemyCircularSpacer : MonoBehaviour
	{

		#region Fields 

		[SerializeField]
		private Transform m_origin = null;

		[SerializeField]
		private List<EnemySpacingConfig> m_spacingConfigs = new List<EnemySpacingConfig>();

		private Dictionary<EnemyType, List<CicularSpacingSpot>> m_enemyTypeToSpots = new Dictionary<EnemyType, List<Gameplay.CicularSpacingSpot>>();

		#endregion

		#region Methods 

		private void OnEnable()
		{
			Initialize();
		}

		private void OnDrawGizmos()
		{
			foreach (KeyValuePair<EnemyType, List<CicularSpacingSpot>> pair in m_enemyTypeToSpots)
			{
				EnemySpacingConfig config = m_spacingConfigs.Find(item => item.enemyType == pair.Key);

				if (!config.drawGizmos)
					continue;

				for (int i = 0; i < pair.Value.Count; i++)
				{
					CicularSpacingSpot spot = pair.Value[i];
					Gizmos.color = spot.isAvailable ? config.gizmosColor : Color.gray;
					if (spot.isAvailable)
						Gizmos.DrawSphere(spot.position, config.gizmosRadius);
				}
			}
		}

		public void Initialize()
		{
			m_enemyTypeToSpots.Clear();

			float anglePerEnemy;
			float angle;
			Vector3 direction = Vector3.zero;
			Vector3 localPosition;

			foreach (EnemySpacingConfig config in m_spacingConfigs)
			{
				anglePerEnemy = 360f / config.count;

				List<CicularSpacingSpot> spots = new List<CicularSpacingSpot>();
				for (int i = 0; i < config.count; i++)
				{
					angle = Mathf.Deg2Rad * (config.angle + anglePerEnemy * i);
					direction.x = Mathf.Cos(angle);
					direction.y = Mathf.Sin(angle);
					localPosition = direction * config.radius;
					CicularSpacingSpot spot = new CicularSpacingSpot(m_origin.position, localPosition);
					spots.Add(spot);
				}

				m_enemyTypeToSpots.Add(config.enemyType, spots);
			}
		}

		public CicularSpacingSpot GetSpot(EnemyType enemyType, Vector3 referencePosition)
		{
			if (m_enemyTypeToSpots.ContainsKey(enemyType))
			{
				List<CicularSpacingSpot> spots = m_enemyTypeToSpots[enemyType];
				Vector3 directionToReference = referencePosition - m_origin.position;
				float currentDot;
				float closestDot = 0;
				CicularSpacingSpot closestSpot = null;

				foreach (CicularSpacingSpot spot in spots)
				{
					currentDot = Vector3.Dot(directionToReference, spot.direction);
					if (currentDot < closestDot)
					{
						closestDot = currentDot;
						closestSpot = spot;
					}
				}
				return closestSpot;
			}
			else
				return null;
		}


		#endregion

	}

}