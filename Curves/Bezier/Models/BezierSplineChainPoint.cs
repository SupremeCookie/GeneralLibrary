using System.Collections.Generic;

namespace Curves
{
	[System.Serializable]
	public class BezierSplineChainPoint
	{
		public GUID ID;
		public BezierSplinePointData PointData;
		public List<IndexedDistance> ArcLengthsToNextPoint;

		/// <summary>
		/// This is the TOTAL distance since start, not just delta distance till previous point.
		/// </summary>
		public float Distance;

		/// <summary>
		/// Distance used by Closed Loop bezier splines, not used by any other point than the first
		/// </summary>
		public float ClosedLoopDistance = -1;

		public BezierSplineChainPoint()
		{
			ID = new GUID();
			PointData = new BezierSplinePointData();
			Distance = 0;
			ArcLengthsToNextPoint = new List<IndexedDistance>();
		}

		public override string ToString()
		{
			return string.Format($"(Point:{this.PointData.ToString()}), (Distance:{this.Distance.ToString()}), (ArcLenghtsCount:{this.ArcLengthsToNextPoint.Count.ToString()})");
		}
	}

	public static class BezierSplineChainPointExtensions
	{
		public static BezierSplineChainPoint Copy(this BezierSplineChainPoint input)
		{
			var result = new BezierSplineChainPoint();

			result.ArcLengthsToNextPoint = input.ArcLengthsToNextPoint.Copy();
			result.ClosedLoopDistance = input.ClosedLoopDistance;
			result.Distance = input.Distance;
			result.ID = input.ID;
			result.PointData = input.PointData.Copy();

			return result;
		}

		public static List<BezierSplineChainPoint> Copy(this List<BezierSplineChainPoint> input)
		{
			var result = new List<BezierSplineChainPoint>(input.Count);

			for (int i = 0; i < input.Count; ++i)
			{
				result.Add(input[i].Copy());
			}

			return result;
		}

		public static void Scale(this BezierSplineChainPoint input, float scaleFactor)
		{
			var position = input.PointData.LocalPosition;
			var controlPoints = input.PointData.ControlPoints.Copy();


			position *= scaleFactor;
			for (int i = 0; i < controlPoints.Count; ++i)
			{
				controlPoints[i].LocalPosition *= scaleFactor;
			}


			input.PointData.LocalPosition = position;
			input.PointData.ControlPoints = controlPoints;
		}
	}
}
