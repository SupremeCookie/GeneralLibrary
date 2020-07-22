using UnityEngine;

public static class Matrices
{
	public static Vector2 ApplyRotationMatrix(Vector2 target, float anglesInDegrees)
	{
		float theta = Mathf.Deg2Rad * anglesInDegrees;
		return new Vector2
		{
			x = (target.x * Mathf.Cos(theta)) - (target.y * Mathf.Sin(theta)),
			y = (target.x * Mathf.Sin(theta)) + (target.y * Mathf.Cos(theta)),
		};
	}
}