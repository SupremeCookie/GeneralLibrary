using UnityEngine;

public static class CustomRandomExtensions
{
	public static float NextFloat_100(this CustomRandom rand)
	{
		Debug.Assert(rand != null, "Can't execute NextFloat_100, rand is null");
		return rand.NextFloat() % 100.0f;
	}
}
