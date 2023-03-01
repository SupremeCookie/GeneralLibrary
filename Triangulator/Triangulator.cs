//#define LOGGING
//#define MEASURING_PERFORMANCE
#define MEASURING_STATIC_PERFORMANCE

using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// TODO DK: Add unit testing
// Note DK: So the performance of the triangulator approximates O(n^2) each extra vertex to triangulate makes the algorithm run a whole lot slower
// I will not (2023-3-1) be solving this, the solution is to prepare the data differently.
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
#if MEASURING_STATIC_PERFORMANCE
			utcTicks = System.DateTime.UtcNow.Ticks;
#endif

			StorePerformanceMeasurement("Triangulate -- Start");
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

			StorePerformanceMeasurement("Triangulate -- Before Initial Data");

			CacheVertexOrder();
			CacheConvexAndReflexVertices();
			MakeEars();

			StorePerformanceMeasurement("Triangulate -- After Initial Data");

			int tracker = 0;
			int maxTracker = vertexCount * 5;   // To ensure we don't get stuck in infinite loops, we add a maximum iteration count.

			// TODO DK: Measure total performance here.
			// Can use a performance measurer, and clear on first step of while loop. Then each time we go through we measure how much each step costs.
			// Then we store that at the end of the loop, and tally that up to a big total. 
			// The big total is total time across all loop counts, so we display that, and then tracker, and then divide by tracker.
			// That way we can somewhat see how long each step takes.
			while (vertexOrder.Count >= minimumVerticesForTriangle)
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

			StorePerformanceMeasurement($"Triangulate -- After triangulation, iterations: {tracker}");

			Debug.Assert(vertexOrder.Count < minimumVerticesForTriangle, $"We stopped iterating, but we still have {minimumVerticesForTriangle} or more vertices available, {vertexOrder.Count}");

#if LOGGING
			Debug.Log($"Fully triangulated the shape, had to iterate: {tracker} times");
#endif

			StorePerformanceMeasurements();
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





#if MEASURING_PERFORMANCE || MEASURING_STATIC_PERFORMANCE
		public const int PerformanceMeasurerWindowIndex = 1;
		private PerformanceMeasurer performanceMeasurer;
#endif

#if MEASURING_STATIC_PERFORMANCE
		public static string MeasurementPrefix;
		private static long utcTicks;
		private static Dictionary<string, double> measuredTimings;
#endif


		private void StorePerformanceMeasurement(string id)
		{
#if !UNITY_EDITOR
            return;
#endif

			if (performanceMeasurer == null)
			{
				performanceMeasurer = new PerformanceMeasurer();
			}

#if MEASURING_PERFORMANCE
            performanceMeasurer.StoreEntry(id);
#endif

#if MEASURING_STATIC_PERFORMANCE
			var key = $"{MeasurementPrefix}_{id}";
			if (measuredTimings == null)
			{
				const int defaultKeys = 5;
				measuredTimings = new Dictionary<string, double>(defaultKeys);
			}

			double addedTime = System.TimeSpan.FromTicks(System.DateTime.UtcNow.Ticks - utcTicks).TotalSeconds;
			utcTicks = System.DateTime.UtcNow.Ticks;
			if (measuredTimings.ContainsKey(key))
			{
				measuredTimings[key] += addedTime;
			}
			else
			{
				measuredTimings.Add(key, addedTime);
			}
#endif
		}

		private void StorePerformanceMeasurements()
		{
#if !UNITY_EDITOR
            return;
#endif

#if MEASURING_STATIC_PERFORMANCE
			var timingMetrics = new List<PerformanceMeasurer.MetricMeasurement>(measuredTimings.Count);
			var metrics = PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex];
			if (metrics == null)
			{
				metrics = new List<PerformanceMeasurer.MetricMeasurement>();
			}

			foreach (var kvp in measuredTimings)
			{
				var existingItem = PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex]?.FirstOrDefault(s => s?.name.Equals(kvp.Key) ?? false);
				if (existingItem != null)
				{
					metrics[metrics.IndexOf(existingItem)].durationSincePrevious += kvp.Value;
				}
				else
				{
					metrics.Add(new PerformanceMeasurer.MetricMeasurement
					{
						name = kvp.Key,
						durationSincePrevious = kvp.Value,
						timeTicks = -1,
					});
				}
			}

			PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex] = metrics;
#endif

#if MEASURING_PERFORMANCE
            var metrics = performanceMeasurer.GetMetrics();

            if (PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex] == null)
            {
                PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex] = new List<PerformanceMeasurer.MetricMeasurement>();
            }

            var measurementCount = performanceMeasurer.Count();
            StorePerformanceMeasurement($"Total Triangulations So Far: " +
                $"{(PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex].Count / (measurementCount + 1)) + 1}");

            PerformanceMeasurererWindowUtility.metricData[PerformanceMeasurerWindowIndex].AddRange(metrics);
#endif
		}

		public static void ResetTheMeasurements()
		{
			measuredTimings = new Dictionary<string, double>();
		}
	}
}