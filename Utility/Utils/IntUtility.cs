using UnityEngine;

public partial class Utility
{
	public static int SignedModulo(int val, int max)
	{
		if (val <= 0)
			return (max - Mathf.Abs(val)) % max;

		if (val < max)
			return val;

		return val % max;
	}

	public static int Modulo(int val, int max)
	{
		int addition = 0;
		if (val < 0)
		{
			int multiplication = Mathf.CeilToInt(Mathf.Abs(val) / (float)max);
			addition = multiplication * max;
		}

		return (val + addition) % max;
	}
}