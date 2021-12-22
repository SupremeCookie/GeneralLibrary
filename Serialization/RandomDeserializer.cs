using Newtonsoft.Json;
using System.Collections.Generic;

public static class RandomDeserializer
{
	public static void Deserialize_RandomContainer(string data)
	{
		Dictionary<string, CustomRandom> result;
		result = JsonConvert.DeserializeObject<Dictionary<string, CustomRandom>>(data);

		CustomRandomContainer.OverwriteCustomRandoms(result);

		UnityEngine.Debug.Log("RandomDeserializer -- Deserialized RandomContainer");
	}

	public static void Deserialize_SeedGenerator(string data)
	{
		Dictionary<string, int> result;
		result = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);

		SeedGenerator.OverwriteSeeds(result);

		UnityEngine.Debug.Log("RandomDeserializer -- Deserialized SeedGenerator");
	}
}