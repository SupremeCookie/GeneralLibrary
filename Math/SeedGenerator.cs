public static class SeedGenerator
{
	public static int GameSeed { get; private set; }
	public static int MathSeed { get; private set; }
	public static int UtilitySeed { get { return MathSeed; } }

	private static MersenneTwister _seedGen;

	static SeedGenerator()
	{
		var random = new System.Random();
		var randomSeed = random.Next(int.MinValue, int.MaxValue);
		uint randomSeedUInt = 0;

		unchecked
		{
			randomSeedUInt = (uint)randomSeed;
		}

		_seedGen = new MersenneTwister(randomSeedUInt);


		GameSeed = _seedGen.NextInt();
		MathSeed = _seedGen.NextInt();
	}
}
