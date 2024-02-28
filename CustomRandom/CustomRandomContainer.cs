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
			int seedKey = SeedGenerator.GameSeed;
			bool hasAdded = _customRandoms.TryAdd(key, new CustomRandom(seedKey));  // Note DK: hasAdded being false means the key is already present which could happen in a multi-threaded environment. (which is why they're concurrent dictionaries)
		}

		return _customRandoms[key];
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
				bool hasAdded = _customRandoms.TryAdd(rand.Key, rand.Value);
			}
		}
	}
}