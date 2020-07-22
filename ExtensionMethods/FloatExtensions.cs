using UnityEngine;

public static class FloatExtensions
{
	private const float FLOATS_CLOSE_TOGETHER = 0.01f;
	private const float TRUNC_MULT = 100f;

	public static float Truncate(this float val)
	{
		return Mathf.Round(TRUNC_MULT * val) / TRUNC_MULT;
	}

	public static float RoundToNearest(this float val, float roundTo)
	{
		return Mathf.Round(val / roundTo) * roundTo;
	}

	public static bool IsCloseTo(this float val, float closeTo)
	{
		return (Mathf.Abs(val - closeTo) < FLOATS_CLOSE_TOGETHER);
	}

	public static float Squared(this float first)
	{
		return Mathf.Pow(first, 2);
	}

	public static float Normalized(this float input)
	{
		if (input == 0)
		{
			return 0;
		}

		float copy = input;
		return copy / Mathf.Abs(copy);
	}
}
