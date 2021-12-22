using System.Collections.Generic;

public static class DictionaryExtensions
{
	public static void Register<T, U>(this Dictionary<T, List<U>> dict, T key, U value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key].Add(value);
		}
		else
		{
			dict.Add(key, new List<U> { value });
		}
	}

	public static void RegisterAndPrealloc<T, U>(this Dictionary<T, List<U>> dict, T key, U value, int size = 500)
	{
		if (dict.ContainsKey(key))
		{
			dict[key].Add(value);
		}
		else
		{
			var newList = new List<U>(size);
			newList.Add(value);
			dict.Add(key, newList);
		}
	}

	public static void Register<T, U>(this Dictionary<T, U> dict, T key, U value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] = value;
		}
		else
		{
			dict.Add(key, value);
		}
	}


	public static bool IsNullOrEmpty<T, U>(this Dictionary<T, U> dict)
	{
		if (dict == null || (dict.Keys == null || dict.Keys.Count == 0))
		{
			return true;
		}

		return false;
	}
}