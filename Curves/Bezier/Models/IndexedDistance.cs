using System.Collections.Generic;

namespace Curves
{
	[System.Serializable]
	public class IndexedDistance
	{
		public int Index;
		public float Distance;


		public override string ToString()
		{
			return $"(Index:{this.Index}), (Distance:{this.Distance})";
		}
	}

	public static class IndexedDistanceExtensions
	{
		public static IndexedDistance Copy(this IndexedDistance input)
		{
			var result = new IndexedDistance();

			result.Index = input.Index;
			result.Distance = input.Distance;

			return result;
		}

		public static List<IndexedDistance> Copy(this List<IndexedDistance> input)
		{
			var result = new List<IndexedDistance>(input.Count);

			for (int i = 0; i < input.Count; ++i)
			{
				result.Add(input[i].Copy());
			}

			return result;
		}
	}
}
