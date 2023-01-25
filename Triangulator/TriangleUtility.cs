//#define USE_RANDOM_TRIANGLE_METHOD
//#define USE_ROTARY_WALKING_METHOD
#define USE_PROPER_MATHS

using System.Collections.Generic;
using UnityEngine;

namespace Triangulator
{
	public enum CWType { CW, CCW, }    // Clock-Wise, Counter-Clock-Wise

#if USE_ROTARY_WALKING_METHOD
	public enum RotaryEvaluationPointMode { Baricenter, RandomVertex, };
#endif

	public static class TriangleUtility
	{
#if USE_ROTARY_WALKING_METHOD
		public static CWType CheckClockwiseTypeOfLoop(in List<Vector2> meshVertexLoop, RotaryEvaluationPointMode evalMode)
#else
		public static CWType CheckClockwiseTypeOfLoop(in List<Vector2> meshVertexLoop)
#endif
		{
#if USE_RANDOM_TRIANGLE_METHOD
			Vector2[] randomTriangle = new Vector2[]
			{
				meshVertexLoop[0],
				meshVertexLoop[1],
				meshVertexLoop[2],
			};

			return GetCWTypeForTriangle(randomTriangle);
#endif

#if USE_ROTARY_WALKING_METHOD || USE_PROPER_MATHS
#if USE_PROPER_MATHS
			return GetCWTypeForVertexLoop(in meshVertexLoop);
#else
			return GetCWTypeForVertexLoop(in meshVertexLoop, evalMode);
#endif
#endif

		}

#if USE_ROTARY_WALKING_METHOD
		public static CWType CheckClockwiseTypeOfLoop(in List<Coordinates> meshVertexLoop, RotaryEvaluationPointMode evalMode)
#else
		public static CWType CheckClockwiseTypeOfLoop(in List<Coordinates> meshVertexLoop)
#endif
		{
#if USE_RANDOM_TRIANGLE_METHOD
			Vector2[] randomTriangle = new Vector2[]
			{
				meshVertexLoop[0].ToVector2(),
				meshVertexLoop[1].ToVector2(),
				meshVertexLoop[2].ToVector2(),
			};

			return GetCWTypeForTriangle(randomTriangle);
#endif

#if USE_ROTARY_WALKING_METHOD || USE_PROPER_MATHS
			var meshVertexLoopConverted = new List<Vector2>(meshVertexLoop.Count);
			for (int i = 0; i < meshVertexLoop.Count; ++i)
			{
				meshVertexLoopConverted.Add(meshVertexLoop[i].ToVector2());
			}

#if USE_PROPER_MATHS
			return GetCWTypeForVertexLoop(in meshVertexLoopConverted);
#else
			return GetCWTypeForVertexLoop(in meshVertexLoopConverted, evalMode);
#endif
#endif
		}

#if USE_RANDOM_TRIANGLE_METHOD
		private static CWType GetCWTypeForTriangle(Vector2[] triangle)
		{
			Debug.Assert(triangle.Length == 3, $"Triangle is invalid, length: {triangle.Length}");

			float vertexEffect = 1.0f / triangle.Length;
			Vector2 bariCenter = Vector2.zero;
			for (int i = 0; i < triangle.Length; ++i)
			{
				bariCenter += triangle[i] * vertexEffect;
			}

			var vectorOne = triangle[0] - bariCenter;
			var vectorTwo = triangle[1] - bariCenter;

			var angleBetween = Utility.SignedThetaBetweenVectors(in vectorOne, in vectorTwo);
			bool isCW = angleBetween < 0;

			//Debug.Log($"Checking CWType,  Vector1: {vectorOne}, Vector2: {vectorTwo},  randomTriangle: {triangle[0]}, {triangle[1]}, {triangle[2]}. " +
			//	$"angleBetween: {angleBetween}, isCW: {isCW}");

			return isCW ? CWType.CW : CWType.CCW;
		}
#endif

#if USE_ROTARY_WALKING_METHOD
		private static CWType GetCWTypeForVertexLoop(in List<Vector2> vertexLoop, RotaryEvaluationPointMode evalMode)
		{
			// O(2n);
			Debug.Assert(!vertexLoop.IsNullOrEmpty(), $"Can't walk the vertexloop as its null or empty");

			Vector2 evaluationPoint = GetEvaluationPoint(evalMode, in vertexLoop);

			// So we take baricenter to vertex[0] as base.
			// Then each vector between baricenter and vertex[i], we check against the previous vertex. We then add all the values together to see if we've got a ccw or cw 


			float totalRotationAngles = 0f;
			Vector2 previousVector = vertexLoop[0] - evaluationPoint;

			for (int i = 1; i < vertexLoop.Count + 1; ++i)
			{
				Vector2 deltaToPoint = vertexLoop[i % vertexLoop.Count] - evaluationPoint;
				float newAngles = Vector2.SignedAngle(previousVector, deltaToPoint);
				totalRotationAngles += newAngles;

				previousVector = deltaToPoint;
			}

			if (totalRotationAngles.IsCloseTo(360, 0.005f))
			{
				return CWType.CCW;
			}
			else if (totalRotationAngles.IsCloseTo(-360, 0.005f))
			{
				return CWType.CW;
			}

			if (totalRotationAngles.IsCloseTo(0, 0.005f))
			{
				Debug.LogWarning($"The total rotation angles are close to 0, the way this occurs, is if the baricenter is outside of the shape, {totalRotationAngles}");
				return CWType.CW;
			}

			Debug.LogError($"TotalRotationAngles are: {totalRotationAngles}, this is not a defined case.");
			return CWType.CW;
		}

		public static Vector2 GetEvaluationPoint(RotaryEvaluationPointMode evalMode, in List<Vector2> vertexLoop)
		{
			var result = Vector2.zero;

			Vector2 calculateBariCenter(List<Vector2> vertices)
			{
				Vector2 bariCenter = Vector2.zero;
				float influence = 1.0f / vertices.Count;

				for (int i = 0; i < vertices.Count; ++i)
				{
					bariCenter += (vertices[i] * influence);
				}

				return bariCenter;
			}

			switch (evalMode)
			{
				case RotaryEvaluationPointMode.Baricenter:
				{
					result = calculateBariCenter(vertexLoop);
					break;
				}

				case RotaryEvaluationPointMode.RandomVertex:
				{
					var randomVertex = vertexLoop[0];
					var secondVertex = vertexLoop[1];

					var delta = secondVertex - randomVertex;

					result = randomVertex + (delta.normalized * 0.1f);
					break;
				}

				default:
				{
					Debug.LogError($"No case defined for evaluation mode: {evalMode}");
					break;
				}
			}

			return result;
		}
#endif

#if USE_PROPER_MATHS
		// This method uses the logic as described in: https://www.element84.com/blog/determining-the-winding-of-a-polygon-given-as-a-set-of-ordered-points
		private static CWType GetCWTypeForVertexLoop(in List<Vector2> vertexLoop)
		{
			// sum_{n=1}^N(x_{n+1} – x_n)(y_{n+1} + y_n)
			// If result is above 0, we are CW, if below 0, we are CCW
			float totalSum = 0f;
			for (int i = 0; i < vertexLoop.Count; ++i)
			{
				var currentVert = vertexLoop[i];
				var nextVert = vertexLoop[(i + 1) % vertexLoop.Count];

				var calcVector = new Vector2(nextVert.x - currentVert.x, nextVert.y + currentVert.y);
				totalSum += calcVector.x * calcVector.y;
			}

			if (totalSum > 0)
			{
				return CWType.CW;
			}
			else
			{
				return CWType.CCW;
			}
		}
#endif
	}
}
