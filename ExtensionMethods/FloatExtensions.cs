using UnityEngine;

public static class FloatExtensions
{
	private const float FLOATS_CLOSE_TOGETHER = 0.0001f;
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
		return IsCloseTo(val, closeTo, FLOATS_CLOSE_TOGETHER);
	}

	public static bool IsCloseTo(this float val, float closeTo, float maxDistance)
	{
		return (Mathf.Abs(val - closeTo) < maxDistance);
	}

	public static float Squared(this float first)
	{
		return Mathf.Pow(first, 2);
	}

	public static float Normalized(this float input)
	{
		return Mathf.Sign(input) * 1f;
	}

	public static int ToMilliSeconds(this float input)
	{
		return (int)input * 1000;
	}
}
