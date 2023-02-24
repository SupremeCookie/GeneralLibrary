//#define LOGGING

using System.Collections.Generic;
using UnityEngine;


// Add unit testing
namespace Triangulator
{
	public class Main
	{
		private List<Vector2> vertexPositions = new List<Vector2>();
		private List<int> indices = new List<int>();

		private List<Vertex> vertexOrder;
		private List<Vertex> convexVertices;
		private List<Vertex> reflexVertices;
		private List<Ear> ears;
		private List<Vertex> removedEarTips;

		public Main(Vector2[] vertices)
		{
			this.vertexPositions = new List<Vector2>(vertices);
		}

		public int[] Triangulate()
		{
#if UNITY_EDITOR
			var cwType = TriangleUtility.CheckClockwiseTypeOfLoop(in vertexPositions, out var metaData);
			//Debug.Log($"cwType for vertices: {vertexPositions.Count} is: CWType.{cwType},   totalSum: {metaData.totalSum}");
			Debug.Assert(metaData.cwType != CWType.Default, $"We got a totalSum of: {metaData.totalSum}, this results in a cwType of: CWType.{metaData.cwType}, this is incorrect");
#endif


			var vertexCount = vertexPositions.Count;

			const int minimumVerticesForTriangle = 3;
			const int vertexCutoff = minimumVerticesForTriangle;
			bool notEnoughVertices = vertexCount < vertexCutoff;
			if (notEnoughVertices)
			{
				Debug.Assert(indices != null, $"Can't cast ToArray on a list that's null, please fix");
				Debug.Log($"Stopped triangulation because there's less than {vertexCutoff} vertices left, vertexCount {vertexCount}, indexCount {indices?.Count ?? -1}");
				return indices.ToArray();
			}

			indices = new List<int>(Mathf.FloorToInt(vertexCount * 1.5f)); // Note DK: A base capacity somewhat close to our expected result to reduce collection resizing. 1.5 found through tests.
			ears = new List<Ear>(vertexCount - 2);

			vertexOrder = new List<Vertex>(vertexCount);
			convexVertices = new List<Vertex>(vertexCount);
			reflexVertices = new List<Vertex>(vertexCount);  // Note DK: Using the term Reflex as it's more distinct when skimming over the code
			removedEarTips = new List<Vertex>(vertexCount);

			CacheVertexOrder();
			CacheConvexAndReflexVertices();
			MakeEars();

			int tracker = 0;
			int maxTracker = vertexCount * 5;   // To ensure we don't get stuck in infinite loops, we add a maximum iteration count.

			// TODO DK: Measure total performance here.
			// Can use a performance measurer, and clear on first step of while loop. Then each time we go through we measure how much each step costs.
			// Then we store that at the end of the loop, and tally that up to a big total. 
			// The big total is total time across all loop counts, so we display that, and then tracker, and then divide by tracker.
			// That way we can somewhat see how long each step takes.
			while (vertexOrder.Count > minimumVerticesForTriangle)
			{
				if (tracker > maxTracker)
				{
					Debug.LogError($"Went over maxTracker: {maxTracker},  currentTracker: {tracker}");
					break;
				}

				// It may occur we need to make new ears, instead of creating them when updating neighbors of snipped ears, 
				// exact cause for this necessity isn't clear yet, needs to be investigated and fixed.
				if (ears.Count == 0)
				{
					Debug.LogWarning($"Had to create new ears, vertexOrder remaining: {vertexOrder.Count}, tracker: {tracker}");
					MakeEars();

					if (ears.Count <= 0)
					{
						Debug.LogError($"Didn't create any new ears, abort triangulation");
						break;
					}
				}


				var ear = ears[0];
				var earTip = ear.vertices[0];
				var earTipNeighbors = new Vertex[2] { ear.vertices[1], ear.vertices[2] };

				ears.RemoveAt(0);
				indices.AddRange(ear.vertices.GetIDs());
				convexVertices.Remove(earTip);
				vertexOrder.Remove(earTip);
				removedEarTips.Add(earTip);

				UpdateNeighborVertices(ref earTipNeighbors);
				UpdateNeighborEars(ref earTipNeighbors);

				tracker++;
			}

			Debug.Assert(vertexOrder.Count < minimumVerticesForTriangle, $"We stopped iterating, but we still have {minimumVerticesForTriangle} or more vertices available, {vertexOrder.Count}");

#if LOGGING
			Debug.Log($"Fully triangulated the shape, had to iterate: {tracker} times");
#endif

			return indices.ToArray();
		}

		public float CalculateTotalSurfaceArea()
		{
			throw new System.NotImplementedException($"Have not written an implementation for calculating the Triangulator's resulting Area yet");
		}


		private void UpdateNeighborVertices(ref Vertex[] vertices)
		{
			// Needed as the removed ear necessitates re-evaluation of neighbor's angles
			for (int n = 0; n < vertices.Length; ++n)
			{
				var currentNeighbor = vertices[n];
				if (convexVertices.Contains(currentNeighbor)) { convexVertices.Remove(currentNeighbor); }
				if (reflexVertices.Contains(currentNeighbor)) { reflexVertices.Remove(currentNeighbor); }
			}

			for (int n = 0; n < vertices.Length; ++n)
			{
				EvaluateVertexAngle(in vertices[n]);
			}
		}

		private void UpdateNeighborEars(ref Vertex[] vertices)
		{
			// Needed as neighbors' ears used to contain the current eartip, the eartip gets removed after this iteration
			for (int e = ears.Count - 1; e >= 0; --e)
			{
				var currentEar = ears[e];
				if (currentEar.ContainsVertices(vertices))
				{
					ears.RemoveAt(e);
				}
			}

			for (int n = 0; n < vertices.Length; ++n)
			{
				var currentNeighbor = vertices[n];
				bool canStoreEar = convexVertices.Contains(currentNeighbor);
				if (canStoreEar)
				{
					TryStoreEar(currentNeighbor);
				}
			}
		}


		private void CacheVertexOrder()
		{
			var count = vertexPositions.Count;
			for (int i = 0; i < count; ++i)
			{
				vertexOrder.Add(new Vertex(i, vertexPositions[i]));
			}
		}

		private void CacheConvexAndReflexVertices()
		{
			var count = vertexPositions.Count;
			for (int i = 0; i < count; ++i)
			{
				EvaluateVertexAngle(vertexOrder[i]);
			}
		}

		private void MakeEars()
		{
			for (int i = 0; i < convexVertices.Count; ++i)
			{
				Vertex currentConvexVertex = convexVertices[i];
				TryStoreEar(currentConvexVertex);
			}
		}

		private void TryStoreEar(Vertex currentVertex)
		{
			GetNeighborVertices(currentVertex, out Vertex nextVertex, out Vertex previousVertex);
			Ear newPotentialEar = new Ear(currentVertex, nextVertex, previousVertex);

			bool anyPointLiesWithinEar = EarUtility.AnyPointLiesWithinEar(in newPotentialEar, reflexVertices);
			bool canStoreEar = !anyPointLiesWithinEar;
			if (canStoreEar)
			{
				ears.Add(newPotentialEar);
			}
		}

		private void EvaluateVertexAngle(in Vertex currentVertex)
		{
			GetNeighborVertices(in currentVertex, out var previousVertex, out var nextVertex);

			Vector2 currentToPrevious = previousVertex.position - currentVertex.position;
			Vector2 currentToNext = nextVertex.position - currentVertex.position;

			float thetaAngle = Utility.SignedThetaBetweenVectors(currentToNext, currentToPrevious);
			float angleInDegrees = Mathf.Rad2Deg * thetaAngle;
			bool isReflex = angleInDegrees <= 0.01f || angleInDegrees >= 179.99f;

			if (isReflex)
			{
				reflexVertices.Add(currentVertex);
			}
			else
			{
				convexVertices.Add(currentVertex);
			}
		}

		private void GetNeighborVertices(in Vertex currentVertex, out Vertex previousVertex, out Vertex nextVertex)
		{
			var count = vertexOrder.Count;
			var vertexIndex = vertexOrder.IndexOf(currentVertex);

			nextVertex = vertexOrder[(vertexIndex + 1) % count];
			previousVertex = vertexOrder[((vertexIndex - 1) + count) % count];
		}
	}
}