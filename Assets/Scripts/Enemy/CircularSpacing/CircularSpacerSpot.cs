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

	public class CircularSpacerSpot
	{
		public Transform origin;
		public Vector3 direction;
		public Vector3 localPosition;
		public bool isAvailable = true;

		public Vector3 position => origin.position + localPosition;

		public CircularSpacerSpot(Transform origin, Vector3 localPosition)
		{
			this.origin = origin;
			this.localPosition = localPosition;
			this.direction = localPosition.normalized;
		}
	}
}