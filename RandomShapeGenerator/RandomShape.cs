using System.Collections.Generic;
using UnityEngine;

namespace RandomShapeGenerator
{
	// Bunch of vector2 positions
	// List of indices to know the order of positions
	// center position	(average of all positions)
	[System.Serializable]
	public class RandomShape
	{
		public List<Vector2> Positions;
		public Vector2 Center;
	}

	public static class RandomShapeExtensionMethods
	{
		public static RandomShape Copy(this RandomShape original)
		{
			var result = new RandomShape();
			result.Center = original.Center;
			result.Positions = new List<Vector2>(original.Positions);

			return result;
		}

		public static void AddOffset(this RandomShape shape, Vector2 offset)
		{
			shape.Center += offset;

			for (int i = 0; i < shape.Positions.Count; ++i)
			{
				shape.Positions[i] += offset;
			}
		}

		public static bool IsInShape(this RandomShape shape, Vector2 point)
		{
			var points = shape.Positions;

			float totalRotationAngles = 0f;
			Vector2 previousVector = points[0] - point;

			for (int i = 1; i < points.Count + 1; ++i)
			{
				Vector2 deltaToPoint = points[i % points.Count] - point;
				float newAngles = Vector2.SignedAngle(previousVector, deltaToPoint);
				totalRotationAngles += newAngles;

				previousVector = deltaToPoint;
			}

			totalRotationAngles = Mathf.Abs(totalRotationAngles);   // Note DK: I don't care if the points are clock or counter-clockwise rotationally distributed, so the total angles need to be absed before evaluating.

			bool isCloseTo360 = totalRotationAngles.IsCloseTo(360, 0.5f);
			bool isCloseToZero = totalRotationAngles.IsCloseTo(0, 0.5f);

			Debug.Assert(isCloseTo360 || isCloseToZero, "Total Rotations is not close to either zero or 360:   " + totalRotationAngles);

			return isCloseTo360;
		}

		public static MinMax GetBoundingBox(this RandomShape shape)
		{
			var bounds = new MinMax();
			var points = shape.Positions;

			for (int k = 0; k < points.Count; ++k)
			{
				bounds.TryStoreNewMinOrMax(points[k]);
			}

			return bounds;
		}
	}
}
