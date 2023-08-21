using System.Collections.Generic;

public static class ListExtensions
{
	public static void Shuffle<T>(this IList<T> list)
	{
		var rng = CustomRandomContainer.GetRandomInstance("ListShuffle");
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

	public static bool IsNullOrEmpty<T>(this IList<T> list)
	{
		return list == null || list.Count == 0;
	}

	public static List<T> Populate<T>(this List<T> list) where T : new()
	{
		for (int i = 0; i < list.Capacity; i++)
		{
			list.Add(new T());
		}

		return list;
	}
}