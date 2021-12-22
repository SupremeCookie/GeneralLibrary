using UnityEngine;

namespace RandomShapeGenerator
{
	// Pass in a ShapeStructure
	// Pass in a resolution (points on structure)
	// Get returned a RandomShape.

	public static class RandomShapeGenerator
	{
		private static CustomRandom _rand;
		private static CustomRandom rand
		{
			get
			{
				if (_rand == null)
				{
					_rand = CustomRandomContainer.GetRandom("RandomShapeGen");
				}

				return _rand;
			}
		}

		public static RandomShape Generate(ShapeStructure shapeStruct, int points, Vector2 jitterRange, Vector2 positionJitterFromCenter)
		{
			var result = new RandomShape();

			var resultingPoints = shapeStruct.GetPointsOnShape(points, jitterRange);
			int resultingPointCount = resultingPoints.Count;
			float weightPerPoint = 1.0f / resultingPointCount;

			result.Positions = resultingPoints;

			for (int i = 0; i < resultingPointCount; ++i)
			{
				result.Center += resultingPoints[i] * weightPerPoint;
			}


			bool hasCenterPositionJitter = positionJitterFromCenter.sqrMagnitude > float.Epsilon;
			if (hasCenterPositionJitter)
			{
				var center = result.Center;
				for (int i = 0; i < resultingPointCount; ++i)
				{
					var currentPoint = resultingPoints[i];
					var delta = currentPoint - center;

					float jitterValue = rand.Range(positionJitterFromCenter);
					var jitteredDelta = delta - (delta * jitterValue);  // Note DK: We remove the resulting delta from the current delta, as we don't want to do anything if the jitter is 0,0

					var resultingPoint = center + jitteredDelta;
					resultingPoints[i] = resultingPoint;
				}
			}


			return result;
		}
	}
}
