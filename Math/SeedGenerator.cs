public static class SeedGenerator
{
	public static int GameSeed { get; private set; }
	public static int MathSeed { get; private set; }
	public static int UtilitySeed { get; private set; }
	public static int DeterministicSeed { get; private set; }

	private static uint randomSeed;
	private static System.Random deterministicSeedGen;

	static SeedGenerator()
	{
		var random = new System.Random();
		var randomSeed = random.Next(int.MinValue, int.MaxValue);
		uint randomSeedUInt = 0;

		unchecked
		{
			randomSeedUInt = (uint)randomSeed;
		}

		SeedGenerator.randomSeed = randomSeedUInt;

		var seedGen = new MersenneTwister(randomSeedUInt);

		DeterministicSeed = seedGen.NextInt();
		deterministicSeedGen = new System.Random(DeterministicSeed);

		GameSeed = deterministicSeedGen.Next();
		MathSeed = deterministicSeedGen.Next();
		UtilitySeed = deterministicSeedGen.Next();
	}

	public static int GetRandomSeed()
	{
		return deterministicSeedGen.Next();
	}
}
