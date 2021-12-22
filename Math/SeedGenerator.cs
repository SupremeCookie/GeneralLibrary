﻿using RandomSerialization;
using System.Collections.Generic;

public static class SeedGenerator
{
	public static int GameSeed { get; private set; }
	public static int MathSeed { get; private set; }
	public static int UtilitySeed { get; private set; }
	public static int DeterministicSeed { get; private set; }

	private static uint randomSeed;
	private static MersenneTwister _seedGen;
	private static System.Random _deterministicSeedGen;

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

		_seedGen = new MersenneTwister(randomSeedUInt);

		DeterministicSeed = _seedGen.NextInt();
		_deterministicSeedGen = new System.Random(DeterministicSeed);

		GameSeed = _deterministicSeedGen.Next();
		MathSeed = _deterministicSeedGen.Next();
		UtilitySeed = _deterministicSeedGen.Next();
	}

	public static int GetRandomSeed()
	{
		return _deterministicSeedGen.Next();
	}

	// Note DK: When we deserialize/overwrite the seeds, we don't restore the mersennetwister seed. This due to limitations in serialization right now
	// The good thing is, we don't need to restore the mersennetwister either, as its not needed anymore. So it will be nullified.
	public static void OverwriteSeeds(Dictionary<string, int> seeds)
	{
		foreach (var seed in seeds)
		{
			if (seed.Key.Equals(Constants.GameSeedName)) { GameSeed = seed.Value; }
			else if (seed.Key.Equals(Constants.MathSeedName)) { MathSeed = seed.Value; }
			else if (seed.Key.Equals(Constants.UtilitySeedName)) { UtilitySeed = seed.Value; }
			else if (seed.Key.Equals(Constants.MainSeedName)) { _deterministicSeedGen = new System.Random(seed.Value); }
		}

		_seedGen = null;
	}
}
