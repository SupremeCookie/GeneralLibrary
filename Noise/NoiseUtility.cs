namespace Noise
{
	public static class NoiseUtility
	{
		// https://catlikecoding.com/unity/tutorials/noise/

		public static CustomRandom GetRandom(bool useSeededRandom)
		{
			CustomRandom rand;
			if (useSeededRandom)
			{
				rand = CustomRandomContainer.GetRandom("Noise-Utility");
			}
			else
			{
				int seed = SeedGenerator.GetRandomSeed();
				rand = new CustomRandom(seed);
			}

			return rand;
		}

		public static float Smooth(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}
	}
}
