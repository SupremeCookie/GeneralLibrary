using System;
using System.Collections.Generic;
using System.Text;

public static class CustomRandomContainer
{
	private static Dictionary<string, CustomRandom> _customRandoms = new Dictionary<string, CustomRandom>();

	public static CustomRandom GetNewRandom(string key)
	{
		CustomRandom customRandomCreator = GetRandomCreator(key);
		int randomSeed = customRandomCreator.Next();
		return new CustomRandom(randomSeed);
	}

	private static CustomRandom GetRandomCreator(string key)
	{
		if (!_customRandoms.ContainsKey(key))
		{
			int seedKey = KeyToInt(key);
			_customRandoms.Add(key, new CustomRandom(seedKey));
		}

		return _customRandoms[key];
	}

	private static int KeyToInt(string key)
	{
		var stringToByte = Encoding.ASCII.GetBytes(key);
		var string16Length = new byte[16];
		for (int i = 0; i < 16; ++i)
		{
			if (i < stringToByte.Length)
			{
				string16Length[i] = stringToByte[i];
			}
		}

		return BitConverter.ToInt32(string16Length, 0);
	}
}
