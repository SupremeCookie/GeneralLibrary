using System.Collections.Generic;

public static class ListExtensions
{
	public static void Shuffle<T>(this List<T> list)
	{
		System.Random rng = new System.Random();
		int n = list.Count;
		while (n > 0)
		{
			--n;
			int k = rng.Next(n + 1);
			var value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static bool IsNullOrEmpty<T>(this List<T> list)
	{
		return list == null || list.Count == 0;
	}
}