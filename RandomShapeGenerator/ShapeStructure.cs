using Curves;
using System.Collections.Generic;
using UnityEngine;

namespace RandomShapeGenerator
{
	[System.Serializable]
	public class ShapeStructure
	{
		public BezierSpline ShapeSpline;

		private CustomRandom _rand;
		private CustomRandom rand
		{
			get
			{
				if (_rand == null)
				{
					_rand = CustomRandomContainer.GetRandomInstance("ShapeStruct_Rand");
				}

				return _rand;
			}
		}

		// TODO DK: Maybe add a method to generate the randomshape from inside the shapestructure.

		public List<Vector2> GetPointsOnShape(int pointCount)
		{
			Debug.Assert(ShapeSpline != null, "No shape spline to get points off of");

			var result = GetPointsOnShape(pointCount, Vector2.zero);  // 0f makes it have no jitter.

			return result;
		}

		// Jitter is a 0-1 value that allows us to move away from our center to either half extreme, this way 2 points COULD collide together, so we go half-extreme-0.1%	so we don't actually collide.
		public List<Vector2> GetPointsOnShape(int pointCount, Vector2 jitterRange)
		{
			var result = new List<Vector2>();

			float totalDistance = ShapeSpline.TotalDistance;
			float distancePerPoint = totalDistance / pointCount;

			var points = ShapeSpline.GetPoints();
			float closedLength = points[0].ClosedLoopDistance;
			float lastPointLength = points[points.Count - 1].Distance;

			var startPoint = rand.Range(0, totalDistance);
			for (int i = 0; i < pointCount; ++i)
			{
				float addition = (distancePerPoint * i);

				bool isPositiveOffset = rand.NextBool();
				float maxOffset = distancePerPoint * 0.4f;  // 0.5f would be max, but then we might get 1 positive max range, and 1 negative max range, and the points would be on top of eachother. So we pick 0.4f
				float randomJitter = rand.Range(jitterRange);
				float offset = (maxOffset * randomJitter) * (isPositiveOffset ? 1.0f : -1.0f);

				var newPointFloatPos = startPoint + addition + offset;
				newPointFloatPos %= totalDistance;
				if (newPointFloatPos < 0) { newPointFloatPos += totalDistance; }

				//Debug.Log(newPointFloatPos + " : " + addition + " : " + (isPositiveOffset) + " :  " + jitterRange + " :  " + maxOffset + " :  " + randomJitter + " :  " + offset + "  ,   " + totalDistance);

				var newPoint = ShapeSpline.Evaluate(newPointFloatPos);
				result.Add(newPoint);
			}


			return result;
		}
	}
}
