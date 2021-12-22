using System.Collections.Generic;
using UnityEngine;

namespace Curves
{
	[System.Serializable]
	public class BezierSplinePointData
	{
		public GUID ID;
		public Vector2 LocalPosition;
		public List<BezierControlPointData> ControlPoints;

		public BezierSplinePointData()
		{
			ID = new GUID();
			LocalPosition = Vector2.zero;
			ControlPoints = new List<BezierControlPointData>();
		}

		public override string ToString()
		{
			return string.Format($"(Pos:{this.LocalPosition.ToString()}), (ControlPoints:{ControlPoints?.Count ?? 0})");
		}
	}

	public static class BezierSplinePointDataExtensions
	{
		public static BezierSplinePointData Copy(this BezierSplinePointData input)
		{
			var result = new BezierSplinePointData();

			result.ControlPoints = input.ControlPoints.Copy();
			result.ID = input.ID;
			result.LocalPosition = input.LocalPosition;

			return result;
		}

		// Note DK: angle is in degrees.
		public static BezierSplinePointData Rotate(this BezierSplinePointData input, float angle)
		{
			var result = input.Copy();
			result.LocalPosition = input.LocalPosition.RotateVectorAroundVector(Vector2.zero, angle);

			for (int i = 0; i < input.ControlPoints.Count; ++i)
			{
				result.ControlPoints[i].LocalPosition = input.ControlPoints[i].LocalPosition.RotateVectorAroundVector(Vector2.zero, angle);
			}

			return result;
		}

		public static BezierSplinePointData FlipOverXAxis(this BezierSplinePointData input)
		{
			var result = input.Copy();
			result.LocalPosition.y *= -1.0f;

			for (int i = 0; i < input.ControlPoints.Count; ++i)
			{
				result.ControlPoints[i].LocalPosition = input.ControlPoints[i].LocalPosition;
				result.ControlPoints[i].LocalPosition.y *= -1.0f;
			}

			return result;
		}
	}
}
