namespace Bitfrost.Gameplay
{
	using System.Collections.Generic;
	using UnityEngine;

	public class SpotManager : MonoBehaviour
	{
		#region Fields 

		private List<Spot> m_spots = new List<Spot>();

		public bool hasAvailableSpot
		{
			get
			{
				foreach (Spot spot in m_spots)
				{
					if (spot.isAvailable)
						return true;
				}
				return false;
			}
		}

		#endregion

		#region Methods 

		protected void Awake()
		{
			m_spots.Clear();
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent<Spot>(out Spot spot))
					m_spots.Add(spot);
			}
		}

		public Spot GetSpot(Vector3 closestPosition)
		{
			float currentSqrMagnitude;
			float closestSqrMagnitude = 0;
			Spot closestSpot = null;

			foreach (Spot spot in m_spots)
			{
				currentSqrMagnitude = (closestPosition - spot.transform.position).sqrMagnitude;
				if (spot.isAvailable && closestSpot == null ||
					spot.isAvailable && currentSqrMagnitude < closestSqrMagnitude)
				{
					closestSqrMagnitude = currentSqrMagnitude;
					closestSpot = spot;
				}
			}
			return closestSpot;
		}

		#endregion
	}
}