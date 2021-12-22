using System.Collections.Generic;
using UnityEngine;

namespace Curves
{
	[System.Serializable]
	public class BezierControlPointData
	{
		public GUID ID;
		public Vector2 LocalPosition;

		public BezierControlPointData()
		{
			ID = new GUID();
			LocalPosition = Vector2.zero;
		}

		public override string ToString()
		{
			return string.Format($"(Pos:{this.LocalPosition.ToString()})");
		}
	}

	public static class BezierControlPointDataExtensions
	{
		public static BezierControlPointData Copy(this BezierControlPointData input)
		{
			var result = new BezierControlPointData();

			result.ID = input.ID;
			result.LocalPosition = input.LocalPosition;

			return result;
		}

		public static List<BezierControlPointData> Copy(this List<BezierControlPointData> input)
		{
			var result = new List<BezierControlPointData>(input.Count);

			for (int i = 0; i < input.Count; ++i)
			{
				result.Add(input[i].Copy());
			}

			return result;
		}
	}
}
