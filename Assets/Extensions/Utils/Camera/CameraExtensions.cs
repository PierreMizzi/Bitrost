using UnityEngine;

public static class CameraExtensions
{
	public static float MagnitudeToEdge(this Camera camera, float angle)
	{
		float absCosAngle = Mathf.Abs(Mathf.Cos(angle));
		float absSinAngle = Mathf.Abs(Mathf.Sin(angle));

		float magnitude;

		if (camera.pixelWidth / 2f * absSinAngle <= camera.pixelHeight / 2f * absCosAngle)
			magnitude = camera.pixelWidth / 2f / absCosAngle;
		else
			magnitude = camera.pixelHeight / 2f / absSinAngle;

		return magnitude;
	}
}