using System;

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

	private static CustomRandom _shuffleRandom = CustomRandomContainer.GetRandomInstance("ShuffleRandom");
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
	public static void Shuffle<T>(this T[] array, CustomRandom rand)
	{
		int n = array.Length;
		for (int i = 0; i < (n - 1); i++)
		{
			// Use Next on random instance with an argument.
			// ... The argument is an exclusive bound.
			//     So we will not go past the end of the array.
			int r = i + rand.Next(n - i);
			T t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}

	public static bool IsNullOrEmpty<T>(this T[] array)
	{
		return array == null || array.Length == 0;
	}

	public static T[] Populate<T>(this T[] array) where T : new()
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new T();
		}

		return array;
	}


	public static void ForEach(this Array array, System.Action<Array, int[]> action)
	{
		if (array.LongLength == 0) return;
		ArrayTraverse walker = new ArrayTraverse(array);
		do action(array, walker.Position);
		while (walker.Step());
	}

	internal class ArrayTraverse
	{
		public int[] Position;
		private int[] maxLengths;

		public ArrayTraverse(Array array)
		{
			maxLengths = new int[array.Rank];
			for (int i = 0; i < array.Rank; ++i)
			{
				maxLengths[i] = array.GetLength(i) - 1;
			}
			Position = new int[array.Rank];
		}

		public bool Step()
		{
			for (int i = 0; i < Position.Length; ++i)
			{
				if (Position[i] < maxLengths[i])
				{
					Position[i]++;
					for (int j = 0; j < i; j++)
					{
						Position[j] = 0;
					}
					return true;
				}
			}
			return false;
		}
	}
}
