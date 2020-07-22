public static class ArrayExtensions
{
	public static bool Contains<T>(this T[] array, T containItem)
	{
		if (array == null)
		{
			return false;
		}

		for (int i = 0; i < array.Length; i++)
		{
			var entry = array[i];
			if (entry == null)
			{
				continue;
			}

			if (entry.Equals(containItem))
			{
				return true;
			}
		}

		return false;
	}

	private static CustomRandom _shuffleRandom = new CustomRandom(SeedGenerator.UtilitySeed);
	public static void Shuffle<T>(this T[] array)
	{
		int n = array.Length;
		for (int i = 0; i < (n - 1); i++)
		{
			// Use Next on random instance with an argument.
			// ... The argument is an exclusive bound.
			//     So we will not go past the end of the array.
			int r = i + _shuffleRandom.Next(n - i);
			T t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}

	public static bool IsNullOrEmpty<T>(this T[] array)
	{
		return array == null || array.Length == 0;
	}
}
