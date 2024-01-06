using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

// TODO DK: figure out how to get deterministic seeding, and why certain things have become undeterministic in Wrath
public static class CustomRandomContainer
{
	private static ConcurrentDictionary<string, CustomRandom> _customRandoms = new ConcurrentDictionary<string, CustomRandom>();
	public static ConcurrentDictionary<string, CustomRandom> CustomRandoms { get { return _customRandoms; } }

	// Note DK: The problem is though that in Wrath it has been found to be non-deterministic
	//// TODO: test if this is deterministic. Test this by changing dependencies to a CustomRand using a seed.
	//// Note DK: Above test results in being deterministic, as the given seed is constantly returning the same results. It "restarts" basically
	//public static CustomRandom GetRandom(string key)
	//{
	//	CustomRandom customRand = GetRandomInstanceInternal(key);
	//	return new CustomRandom(customRand.Seed);
	//}

	// Note DK: This method, will return the actual instance of the custom random. The GetRandom method will return a fresh CustomRandom instead.
	public static CustomRandom GetRandomInstance(string key)
	{
		return GetRandomInstanceInternal(key);
	}

	private static CustomRandom GetRandomInstanceInternal(string key)
	{
		if (!_customRandoms.ContainsKey(key))
		{
			int seedKey = SeedGenerator.GameSeed;/* SeedGenerator.GetRandomSeed();*/
			bool hasAdded = _customRandoms.TryAdd(key, new CustomRandom(seedKey));  // Note DK: hasAdded being false means the key is already present which could happen in a multi-threaded environment. (which is why they're concurrent dictionaries)
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

	public static void OverwriteCustomRandoms(Dictionary<string, CustomRandom> randoms)
	{
		foreach (var rand in randoms)
		{
			bool isAlreadyPresent = _customRandoms.ContainsKey(rand.Key);
			if (isAlreadyPresent)
			{
				_customRandoms[rand.Key].OverwriteSeed(rand.Value.Seed);
			}
			else
			{
				bool hasAdded = _customRandoms.TryAdd(rand.Key, rand.Value);  // Note DK: hasAdded being false means 
			}
		}
	}
}





/*
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// TODO DK: Output the Dictionary whenever we get into a crash, or when uploading data/a run
// The problem with using the key as a seed, is that we get the same results, every single time. So that's now different. (by storing the key as an accessor, and just grabbing a random key)
public static class CustomRandomContainer
{
	private static Dictionary<string, int> _customRandoms = new Dictionary<string, int>();
	public static Dictionary<string, int> CustomRandoms { get { return _customRandoms; } }

	// TODO: test if this is deterministic. Test this by changing dependencies to a CustomRand using a seed.
	public static CustomRandom GetRandom(string key)
	{
		int randSeed = GetSeed(key);
		return new CustomRandom(new CustomRandom(randSeed).Next());
	}

	private static int GetSeed(string key)
	{
		if (!_customRandoms.ContainsKey(key))
		{
			//int seedKey = KeyToInt(key);
			int seedKey = SeedGenerator.GetRandomSeed();

			_customRandoms.Add(key, seedKey);
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

	public static void OverwriteCustomRandoms(Dictionary<string, int> randoms)
	{
		foreach (var rand in randoms)
		{
			bool isAlreadyPresent = _customRandoms.ContainsKey(rand.Key);
			if (isAlreadyPresent)
			{
				Debug.Log("Overwriting Seed for random: " + rand.Key + "  maybe consider tracking down potential issues");
				_customRandoms[rand.Key] = (rand.Value);
			}
			else
			{
				_customRandoms.Add(rand.Key, rand.Value);
			}
		}
	}
}
*/
