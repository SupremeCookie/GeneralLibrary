using UnityEngine;

public static class LayerMaskExtensions
{
	public static bool Contains(this LayerMask layers, int layer)
	{
		if ((layers & 1 << layer) == 1 << layer)
		{
			return true;
		}

		return false;
	}
}