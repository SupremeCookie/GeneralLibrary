using Newtonsoft.Json;
using RandomSerialization;
using System.Collections.Generic;

public static class RandomSerializer
{
	public static string Serialize_RandomContainer()
	{
		string result = "";

		var randoms = CustomRandomContainer.CustomRandoms;
		result = JsonConvert.SerializeObject(randoms);

		UnityEngine.Debug.Log("RandomSerializer -- Serialized RandomContainer");

		return result;
	}

	public static string Serialize_SeedGenerator()
	{
		string result = "";

		Dictionary<string, int> seeds = new Dictionary<string, int>();
		seeds.Add(Constants.GameSeedName, SeedGenerator.GameSeed);
		seeds.Add(Constants.MathSeedName, SeedGenerator.MathSeed);
		seeds.Add(Constants.UtilitySeedName, SeedGenerator.UtilitySeed);
		seeds.Add(Constants.MainSeedName, SeedGenerator.DeterministicSeed);

		result = JsonConvert.SerializeObject(seeds);

		UnityEngine.Debug.Log("RandomSerializer -- Serialized SeedGenerator");

		return result;
	}
}
