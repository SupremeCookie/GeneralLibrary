//#define DrawDebugStuff
using System.Collections.Generic;
using UnityEngine;

namespace Curves
{
	[System.Serializable]
	public class BezierSpline
	{
		public static int GENERAL_RESOLUTION = 50;


		[SerializeField] private float _totalDistance;
		public float TotalDistance
		{
			get { return _totalDistance; }
			set { _totalDistance = value; }
		}

		public float TotalLength { get { return TotalDistance; } }
		public float Length { get { return TotalDistance; } }

		[SerializeField] private bool _isClosed = false;
		public bool IsClosed
		{
			get { return _isClosed; }
			set { _isClosed = value; this.RecalculateDistance(); }
		}

		[SerializeField] private List<BezierSplineChainPoint> _points;
		public List<BezierSplineChainPoint> Points
		{
			set { _points = value; this.RecalculateDistance(); }
		}



		public BezierSpline()
		{
			Points = new List<BezierSplineChainPoint>();
		}

		public List<BezierSplineChainPoint> GetPoints()
		{
			var copy = new List<BezierSplineChainPoint>(_points.Count);
			for (int i = 0; i < _points.Count; ++i)
			{
				copy.Add(_points[i].Copy());
			}

			return copy;
		}

		public void StoreRelevantData(in List<BezierSplineChainPoint> input)
		{
			for (int i = 0; i < input.Count; ++i)
			{
				_points[i].Distance = input[i].Distance;
				_points[i].ArcLengthsToNextPoint = input[i].ArcLengthsToNextPoint.Copy();
			}
		}
	}



	public static class BezierSplineExtensions
	{
#if DrawDebugStuff
		private const bool DrawDebugStuff = true;
#else
		private const bool DrawDebugStuff = false;
#endif

		public static BezierSpline Copy(this BezierSpline input)
		{
			var result = new BezierSpline();

			result.Points = input.GetPoints();  // Note DK: This calls RecalculateDistance.
			result.TotalDistance = input.TotalDistance;
			result.IsClosed = input.IsClosed;   // Note DK: This calls RecalculateDistance.

			return result;
		}


		private static readonly Vector2 defaultResult = new Vector2(float.PositiveInfinity, float.NegativeInfinity);


		public static Vector2 EvaluateRelative(this BezierSpline spline, float relativeProgress)
		{
			Debug.Assert(relativeProgress >= 0 && relativeProgress <= 1.0f, $"Relative progress is outside of 0-1 space: ({relativeProgress})");
			var result = defaultResult;

			var relativeDistance = spline.TotalDistance * relativeProgress;
			result = spline.Evaluate(relativeDistance);

			Debug.Assert(result.x != defaultResult.x && result.y != defaultResult.y, "We have atleast 1 of the evaluated points at its default value, go fix.");
			return result;
		}

		public static Vector2 Evaluate(this BezierSpline spline, float distance)
		{
			Debug.Assert(distance >= 0 && (distance <= spline.TotalDistance || distance.IsCloseTo(spline.TotalDistance)), "We are trying to grab a point outside of our 0-totalDistance region: " + distance + "   totalDistance:  " + spline.TotalDistance);
			var result = defaultResult;

			int chosenIndex = -1;
			var points = spline.GetPoints();
			for (int i = 0; i < points.Count; ++i)
			{
				var currP = points[i];
				if (currP.Distance > distance)
				{
					Debug.Assert(i > 0, "The first point is somehow further along the spline than the position we requested, that shouldn't be possible");
					chosenIndex = i - 1;
					break;
				}
				else if (currP.Distance.IsCloseTo(distance))
				{
					chosenIndex = i;
					break;
				}
			}

			if (chosenIndex == -1 && spline.IsClosed)
			{
				return HandleClosedLoopEvaluation(in points, in distance);
			}


			return HandleNonClosedLoopEvaluation(in chosenIndex, in points, in distance);
		}

		private static Vector2 HandleNonClosedLoopEvaluation(in int chosenIndex, in List<BezierSplineChainPoint> points, in float distance)
		{
			var result = defaultResult;

			Debug.Assert(chosenIndex >= 0, "Couldn't find a valid point for distance: " + distance);
			bool isLastPoint = chosenIndex == (points.Count - 1);
			if (isLastPoint)
			{
				return points[chosenIndex].PointData.LocalPosition;
			}



			var firstPoint = points[chosenIndex];
			var secondPoint = points[chosenIndex + 1];
			var deltaDist = secondPoint.Distance - firstPoint.Distance;
			var relativeDistance = distance - firstPoint.Distance;

			if (deltaDist.IsCloseTo(0))
			{
				// Note DK: If the delta distance is 0 between two points, we just return the first point.
				return firstPoint.PointData.LocalPosition;
			}


			CalculateProgressBetweenBezierChainPoints(in firstPoint, in relativeDistance, in deltaDist, out var progressBetweenPoints);

			result = CalculatePosition(in firstPoint, in secondPoint, in progressBetweenPoints);

			Debug.Assert(result.x != defaultResult.x && result.y != defaultResult.y, "We have atleast 1 of the evaluated points at its default value, go fix.");
			return result;
		}

		// Note DK: This is only handled if the loop is closed, and only for the last to first spline.
		private static Vector2 HandleClosedLoopEvaluation(in List<BezierSplineChainPoint> points, in float distance)
		{
			var result = defaultResult;

			var firstPointInSpline = points[0];
			Debug.Assert(firstPointInSpline.ClosedLoopDistance >= distance, "The closed loop distance is not bigger than distance:  " + firstPointInSpline.ClosedLoopDistance + " : " + distance + "    We can't find a proper chosen index");

			var firstPoint = points[points.Count - 1];
			var secondPoint = points[0];
			var deltaDist = secondPoint.ClosedLoopDistance - firstPoint.Distance;
			var relativeDistance = distance - firstPoint.Distance;

			if (deltaDist.IsCloseTo(0))
			{
				// Note DK: If the delta distance is 0 between two points, we just return the first point.
				return firstPoint.PointData.LocalPosition;
			}


			CalculateProgressBetweenBezierChainPoints(in firstPoint, in relativeDistance, in deltaDist, out var progressBetweenPoints);

			result = CalculatePosition(in firstPoint, in secondPoint, in progressBetweenPoints);

			Debug.Assert(result.x != defaultResult.x && result.y != defaultResult.y, "We have atleast 1 of the evaluated points at its default value, go fix.");
			return result;
		}




		private static void CalculateProgressBetweenBezierChainPoints(in BezierSplineChainPoint firstPoint, in float relativeDistance, in float deltaDistance, out float progressBetweenPoints)
		{
			progressBetweenPoints = 0f;

			var arcLengths = firstPoint.ArcLengthsToNextPoint;
			if (arcLengths.IsNullOrEmpty())
			{
				// Note DK: If there are no arcLengths, we are simply not an equidistant spline, its still correct, just not equidistant. 
				progressBetweenPoints = relativeDistance / deltaDistance;
				return;
			}

			var arcLengthIndex = ResampleDistanceToArcLengthIndex(in arcLengths, in relativeDistance);

			bool targetCloseToExistingPoint = arcLengths[arcLengthIndex].Distance.IsCloseTo(relativeDistance);
			if (targetCloseToExistingPoint)
			{
				progressBetweenPoints = arcLengthIndex / (float)(arcLengths.Count - 1); // Note DK: -1 so we can reach   1 in relative progress
			}
			else
			{
				float distanceBefore = arcLengths[arcLengthIndex].Distance;
				float distanceAfter = arcLengths[arcLengthIndex + 1].Distance;  // Note DK: Won't ever be reached for final point, as that will be close to existing point.
				float segmentLength = distanceAfter - distanceBefore;

				float segmentFraction = (relativeDistance - distanceBefore) / segmentLength;
				progressBetweenPoints = (arcLengthIndex + segmentFraction) / (arcLengths.Count - 1);
			}
		}

		private static int ResampleDistanceToArcLengthIndex(in List<IndexedDistance> arcLengths, in float targetLength)
		{
			// TODO DK: Make this a binary search.
			for (int i = 0; i < arcLengths.Count; ++i)
			{
				float distance = arcLengths[i].Distance;
				if (distance > targetLength)
				{
					int resultingIndex = arcLengths[i].Index - 1; // Note DK: The first point bigger than the one we need is what we find here. So we -1 it.
					Debug.Assert(resultingIndex >= 0, "Resulting index is smaller than 0, shouldn't happen: " + distance + "--" + targetLength);
					return resultingIndex;
				}
				else if (distance.IsCloseTo(targetLength))
				{
					return i;
				}
			}

			Debug.Assert(false, "Can't find an arclength distance for targetLength: " + targetLength + "   returning 0-index");
			return 0;
		}


		public static void RecalculateDistance(this BezierSpline spline)
		{
			// If the calculated distance is pretty high, redo it and decrease the step size, so at first we check 5 points, if the distance high enough we check 10, then 15, etc.
			// More points is more accuracy, but also slower logic.

			var points = spline.GetPoints();
			if (points.Count < 2)
			{
				spline.TotalDistance = 0f;
			}

			float totalDistance = 0f;

			for (int i = 0; i < points.Count - 1; ++i)
			{
				var currentPoint = points[i];
				var nextPoint = points[i + 1];

				CalculateDistanceBetweenPoints(ref currentPoint, ref nextPoint, out List<IndexedDistance> pointDistances);

				totalDistance += pointDistances[pointDistances.Count - 1].Distance;
				nextPoint.Distance = totalDistance;
				currentPoint.ArcLengthsToNextPoint = pointDistances;
			}


			// Note DK: If we are a closed loop, we circle back from last point to first point, so we need to add that distance along.
			if (spline.IsClosed)
			{
				var firstPoint = points[0];
				var lastPoint = points[points.Count - 1];

				CalculateDistanceBetweenPoints(ref lastPoint, ref firstPoint, out List<IndexedDistance> pointDistances);

				totalDistance += pointDistances[pointDistances.Count - 1].Distance;
				firstPoint.ClosedLoopDistance = totalDistance;
				lastPoint.ArcLengthsToNextPoint = pointDistances;
			}


			spline.TotalDistance = totalDistance;
			spline.StoreRelevantData(points);
		}

		private static void CalculateDistanceBetweenPoints(ref BezierSplineChainPoint currentPoint, ref BezierSplineChainPoint nextPoint, out List<IndexedDistance> pointDistances)
		{
			int resolution = BezierSpline.GENERAL_RESOLUTION;
			resolution += 2;    // Note DK: Adding begin and end node.

			pointDistances = new List<IndexedDistance>(resolution);
			float relativeJump = 1.0f / (resolution - 1);   // Note DK: -1 as the first point doesn't jump.

			Vector2 previousPosition = default(Vector2);
			for (int k = 0; k < resolution; ++k)
			{
				float progress = k / (float)(resolution - 1);
				var newPos = CalculatePosition(currentPoint, nextPoint, progress, DrawDebugStuff);


#if DrawDebugStuff
				if (DrawDebugStuff)
				{
					var newGo = new GameObject($"CalcPoint-{currentPoint.ID}-{k}-{relativeJump * k}--{(newPos - previousPosition).magnitude}");
					newGo.transform.position = newPos;
					var debugDraw = newGo.AddComponent<RogueLike.GizmosDrawPoint>();
					debugDraw.GizmosSize = 0.15f; debugDraw.GizmosColor = Color.green; debugDraw.GizmoType = RogueLike.GizmoType.WireSphere;
				}
#endif


				var newDistance = new IndexedDistance();
				newDistance.Index = k;

				var distance = 0f;
				if (k > 0)
				{
					distance = pointDistances[k - 1].Distance + (newPos - previousPosition).magnitude;  // Note DK: Take last points' distance, and add the new one on top of it.
				}

				newDistance.Distance = distance;
				previousPosition = newPos;

				pointDistances.Add(newDistance);
			}
		}



		public static DistancedWorldPoint[] GetSpline(this BezierSpline spline, float stepSize)
		{
			Debug.Assert(stepSize > 0, "Can't have a stepSize of 0");

			int pointCount = Mathf.CeilToInt(spline.TotalDistance / stepSize);  // Note DK: Ceiling the point count, because we don't necessarily hit the totaldistance with our original stepsize
			float newStepSize = spline.TotalDistance / pointCount;  // Note DK: Recalculate stepsize so you get an equal distribution.

			// Note DK: +1 and <= to include the last point.
			var result = new DistancedWorldPoint[pointCount + 1];

			for (int i = 0; i <= pointCount; ++i)
			{
				float progress = newStepSize * i;
				progress = Mathf.Min(progress, spline.TotalDistance);
				result[i] = new DistancedWorldPoint
				{
					Distance = progress,
					WorldPos = spline.Evaluate(progress),
				};
			}

			return result;
		}

		public static List<DistancedWorldPoint> GetSplineList(this BezierSpline spline, float stepSize)
		{
			Debug.Assert(stepSize > 0, "Can't have a stepSize of 0");

			int pointCount = Mathf.CeilToInt(spline.TotalDistance / stepSize);  // Note DK: Ceiling the point count, because we don't necessarily hit the totaldistance with our original stepsize
			float newStepSize = spline.TotalDistance / pointCount;  // Note DK: Recalculate stepsize so you get an equal distribution.

			// Note DK: +1 and <= to include the last point.
			var result = new List<DistancedWorldPoint>(pointCount + 1);

			for (int i = 0; i <= pointCount; ++i)
			{
				float progress = newStepSize * i;
				progress = Mathf.Min(progress, spline.TotalDistance);
				result.Add(new DistancedWorldPoint
				{
					Distance = progress,
					WorldPos = spline.Evaluate(progress),
				});
			}

			return result;
		}


		private static Vector2 CalculatePosition(in BezierSplineChainPoint firstPoint, in BezierSplineChainPoint secondPoint, in float t, bool drawDebugStuff = false)
		{
			var firstPointData = firstPoint.PointData;
			var secondPointData = secondPoint.PointData;

			var firstControlPointPos = firstPointData.LocalPosition;    // Note DK: Defaulting to firstPoint position if no control points.
			if (firstPointData.ControlPoints.Count > 0)
			{
				firstControlPointPos = (firstPointData.ControlPoints[1]?.LocalPosition ?? Vector2.zero) + firstPointData.LocalPosition;
			}

			var secondControlPointPos = secondPointData.LocalPosition;
			if (secondPointData.ControlPoints.Count > 0)
			{
				secondControlPointPos = (secondPointData.ControlPoints[0]?.LocalPosition ?? Vector2.zero) + secondPointData.LocalPosition;
			}


			// Note DK: Cubic bezier curves have a very simple way of doing the calculation.
			// Instead of running it through the bezier function, we just do a triple layered Lerp, and we're there.

			var firstLayer_A = Vector2.Lerp(firstPoint.PointData.LocalPosition, firstControlPointPos, t);
			var firstLayer_B = Vector2.Lerp(firstControlPointPos, secondControlPointPos, t);
			var firstLayer_C = Vector2.Lerp(secondControlPointPos, secondPoint.PointData.LocalPosition, t);

			var secondLayer_A = Vector2.Lerp(firstLayer_A, firstLayer_B, t);
			var secondLayer_B = Vector2.Lerp(firstLayer_B, firstLayer_C, t);

			var result = Vector2.Lerp(secondLayer_A, secondLayer_B, t);


#if UNITY_EDITOR
			if (drawDebugStuff)
			{
				var newGo = new GameObject($"{t}_First_A");
				newGo.transform.position = firstLayer_A;
				var debugDraw = newGo.AddComponent<GizmosDrawPoint>();
				debugDraw.GizmosSize = 0.1f; debugDraw.GizmosColor = Color.red; debugDraw.GizmoType = GizmoType.Cube;

				newGo = new GameObject($"{t}_First_B");
				newGo.transform.position = firstLayer_B;
				debugDraw = newGo.AddComponent<GizmosDrawPoint>();
				debugDraw.GizmosSize = 0.1f; debugDraw.GizmosColor = Color.red; debugDraw.GizmoType = GizmoType.Cube;

				newGo = new GameObject($"{t}_First_C");
				newGo.transform.position = firstLayer_C;
				debugDraw = newGo.AddComponent<GizmosDrawPoint>();
				debugDraw.GizmosSize = 0.1f; debugDraw.GizmosColor = Color.red; debugDraw.GizmoType = GizmoType.Cube;



				newGo = new GameObject($"{t}_Second_A");
				newGo.transform.position = secondLayer_A;
				debugDraw = newGo.AddComponent<GizmosDrawPoint>();
				debugDraw.GizmosSize = 0.1f; debugDraw.GizmosColor = Color.red; debugDraw.GizmoType = GizmoType.Cube;

				newGo = new GameObject($"{t}_Second_B");
				newGo.transform.position = secondLayer_B;
				debugDraw = newGo.AddComponent<GizmosDrawPoint>();
				debugDraw.GizmosSize = 0.1f; debugDraw.GizmosColor = Color.red; debugDraw.GizmoType = GizmoType.Cube;



				newGo = new GameObject($"{t}_Third_A");
				newGo.transform.position = result;
				debugDraw = newGo.AddComponent<GizmosDrawPoint>();
				debugDraw.GizmosSize = 0.1f; debugDraw.GizmosColor = Color.red; debugDraw.GizmoType = GizmoType.Cube;
			}
#endif


			return result;
		}

		public static void Scale(this BezierSpline input, float scaleFactor)
		{
			var points = input.GetPoints();
			if (points == null)
			{
				return;
			}

			for (int i = 0; i < points.Count; ++i)
			{
				points[i].Scale(scaleFactor);
			}

			input.Points = points;
		}

		/// <summary>
		/// Note DK: The Angle here is in degree angles, not radian-angles or pi-rad-angles.
		/// </summary>
		public static BezierSpline ProjectRotation(this BezierSpline input, float angle)
		{
			var points = input.GetPoints();
			for (int i = 0; i < points.Count; ++i)
			{
				points[i].PointData = points[i].PointData.Rotate(angle);
			}

			var copy = input.Copy();
			copy.Points = points;
			return copy;
		}

		public static BezierSpline ProjectFlip(this BezierSpline input, bool shouldFlipOverXAxis)
		{
			// Note DK: XAxis in this case means flipping over the horizontal line.
			if (!shouldFlipOverXAxis)
			{
				return input;
			}


			var points = input.GetPoints();
			for (int i = 0; i < points.Count; ++i)
			{
				points[i].PointData = points[i].PointData.FlipOverXAxis();
			}

			var copy = input.Copy();
			copy.Points = points;
			return copy;
		}
	}
}