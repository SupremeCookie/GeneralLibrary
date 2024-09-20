using UnityEngine;

public static class TransformExtensions
{
	public static Transform[] GetDirectChildren(this Transform parent)
	{
		int childCount = parent.childCount;
		Transform[] result = new Transform[childCount];
		for (int i = 0; i < childCount; ++i)
		{
			result[i] = parent.GetChild(i).transform;
		}

		return result;
	}
}