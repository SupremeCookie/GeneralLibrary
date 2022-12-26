//#define USE_RANDOM_TRIANGLE_METHOD
#define USE_ROTARY_WALKING_METHOD

using System.Collections.Generic;
using UnityEngine;

namespace Triangulator
{
	public enum CWType { CW, CCW, }    // Clock-Wise, Counter-Clock-Wise

	public static class TriangleUtility
	{
		public static CWType CheckClockwiseTypeOfLoop(in List<Vector2> meshVertexLoop)
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

#if USE_ROTARY_WALKING_METHOD
			return GetCWTypeForVertexLoop(in meshVertexLoop);
#endif
		}

		public static CWType CheckClockwiseTypeOfLoop(in List<Coordinates> meshVertexLoop)
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

#if USE_ROTARY_WALKING_METHOD
			var meshVertexLoopConverted = new List<Vector2>(meshVertexLoop.Count);
			for (int i = 0; i < meshVertexLoop.Count; ++i)
			{
				meshVertexLoopConverted.Add(meshVertexLoop[i].ToVector2());
			}

			return GetCWTypeForVertexLoop(in meshVertexLoopConverted);
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
		private static CWType GetCWTypeForVertexLoop(in List<Vector2> vertexLoop)
		{
			// O(2n);
			Debug.Assert(!vertexLoop.IsNullOrEmpty(), $"Can't walk the vertexloop as its null or empty");

			Vector2 bariCenter = Vector2.zero;
			float influence = 1.0f / vertexLoop.Count;

			for (int i = 0; i < vertexLoop.Count; ++i)
			{
				bariCenter += (vertexLoop[i] * influence);
			}

			// So we take baricenter to vertex[0] as base.
			// Then each vector between baricenter and vertex[i], we check against the previous vertex. We then add all the values together to see if we've got a ccw or cw 


			float totalRotationAngles = 0f;
			Vector2 previousVector = vertexLoop[0] - bariCenter;

			for (int i = 1; i < vertexLoop.Count + 1; ++i)
			{
				Vector2 deltaToPoint = vertexLoop[i % vertexLoop.Count] - bariCenter;
				float newAngles = Vector2.SignedAngle(previousVector, deltaToPoint);
				totalRotationAngles += newAngles;

				previousVector = deltaToPoint;
			}

			if (totalRotationAngles.IsCloseTo(360))
			{
				return CWType.CCW;
			}
			else if (totalRotationAngles.IsCloseTo(-360))
			{
				return CWType.CW;
			}

			Debug.LogError($"TotalRotationAngles are: {totalRotationAngles}, this is not a defined case.");
			return CWType.CW;
		}
#endif
	}
}
