using UnityEngine;

public static class IntExtensions
{
	public static int SignedModulo(this int val, int max)
	{
		if (val <= 0)
			return (max - Mathf.Abs(val)) % max;

		if (val < max)
			return val;

		return val % max;
	}

	public static int Modulo(this int val, int max)
	{
		return Utility.Modulo(val, max);
	}

	public static int ToMilliSeconds(this int input)
	{
		return input * 1000;
	}
}
