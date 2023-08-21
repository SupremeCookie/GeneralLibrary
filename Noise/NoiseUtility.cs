namespace Noise
{
	public static class NoiseUtility
	{
		// https://catlikecoding.com/unity/tutorials/noise/

		public static CustomRandom GetRandom(bool useSeededRandom)
		{
			CustomRandom rand = CustomRandomContainer.GetRandomInstance("Noise-Utility");
			return rand;
		}

		public static float Smooth(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}
	}
}
