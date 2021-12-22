using UnityEngine;

namespace Curves
{
	public static class EditorConstants
	{
		public static readonly Color Text = Color.black;
		public static readonly Color BetweenPointsRay = new Color(217f / 255f, 217f / 255f, 217f / 255f);
		public static readonly Color ControlPointsRay = new Color(191f / 255f, 191f / 255f, 191f / 255f);
		public static readonly Color Point = new Color(255f / 255f, 0f / 255f, 0f / 255f);
		public static readonly Color Curve = new Color(255f / 255f, 255f / 255f, 255f / 255f);
		public static readonly Color[] ControlPoint = new Color[]
		{
			new Color(255f / 255f, 0f / 255f, 255f / 255f),
			new Color(0f / 255f, 255f / 255f, 255f / 255f),
		};
	}
}
