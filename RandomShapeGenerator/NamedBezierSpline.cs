using Curves;

namespace RandomShapeGenerator
{
	[System.Serializable]
	public class NamedBezierSpline
	{
		public string Name;
		public BezierSpline Spline;
		public BezierSplineScriptableObject SplineScriptableObject;
	}
}
