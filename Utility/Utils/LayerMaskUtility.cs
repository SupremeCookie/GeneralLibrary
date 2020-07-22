using UnityEngine;

public partial class Utility
{
	/// <summary>
	/// Only works for single layers. If multiple layers are present, only counts the largest.
	/// </summary>
	public static int ToLayer(int bitmask)
	{
		int result = bitmask > 0 ? 0 : 31;
		while (bitmask > 1)
		{
			bitmask = bitmask >> 1;
			result++;
		}

		return result;
	}
}
