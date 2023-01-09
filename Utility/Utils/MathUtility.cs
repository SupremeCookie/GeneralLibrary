using UnityEngine;

public partial class Utility
{
	/// <summary>
	/// Normally RoundToInt has the issue called Banker's Rounding, this means if the input ends in a 0.5f decimal, then the nearest EVEN number is returned,
	/// whilst this is commonly okay to use, in a few specific situations this may be undesirable. We would like more controlled logic in those cases.
	/// </summary>
	public static int RoundToNearestIntOrCeil(float input)
	{
		int floored = Mathf.FloorToInt(input);
		float cleanedValue = input - floored;
		bool closeToHalf = cleanedValue.IsCloseTo(0.5f, 0.0001f);

		if (closeToHalf)
		{
			return Mathf.CeilToInt(input);
		}

		return Mathf.RoundToInt(input);
	}

	public static Vector2Int RoundToNearestIntOrCeil(Vector2 input)
	{
		return new Vector2Int(RoundToNearestIntOrCeil(input.x), RoundToNearestIntOrCeil(input.y));
	}
}