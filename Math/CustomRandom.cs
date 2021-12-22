using Newtonsoft.Json;
using UnityEngine;

public class CustomRandom
{
	private System.Random _rand;
	[JsonProperty(PropertyName = "Seed")] private int _seed;

	[JsonIgnore] public int Seed { get { return _seed; } }

	public CustomRandom()
	{
		_seed = SeedGenerator.MathSeed;
		_rand = new System.Random(_seed);
	}

	public CustomRandom(int seed)
	{
		_seed = seed;
		_rand = new System.Random(_seed);
	}

	public void OverwriteSeed(int seed)
	{
		_seed = seed;
		_rand = new System.Random(_seed);
	}


	public int Next()
	{
		return _rand.Next();
	}

	public int Next(int maxValue)
	{
		return _rand.Next(maxValue);
	}

	public double NextDouble()
	{
		return _rand.NextDouble();              // System.Random.NextDouble() always returns greater than or 0.0f, or below 1.0f
	}

	/// <summary>
	/// Secretly uses NextDouble, then casts that to float
	/// Since it uses NextDouble, the range is between (inclusive) 0 and (exclusive) 1
	/// </summary>
	public float NextFloat()
	{
		return (float)_rand.NextDouble();       // System.Random.NextDouble() always returns greater than or 0.0f, or below 1.0f
	}

	public void NextBytes(out byte[] byteBuffer)
	{
		byte[] buffer = new byte[] { };
		_rand.NextBytes(buffer);

		byteBuffer = buffer;
	}

	public Vector2 NextVector(/*bool normalised = true*/) // Note DK: New functionality automatically normalises.
	{
		float angle = Range(-1f, 1f) * Mathf.PI; // Note DK: -1 to 1, because we do PI-radials

		Vector2 randomVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

		return randomVector;
	}

	/// <summary>
	/// </summary>
	/// <param name="min">Inclusive</param>
	/// <param name="max">Exclusive</param>
	public int Range(int min, int max)
	{
		return _rand.Next(min, max);
	}

	/// <summary>
	/// </summary>
	/// <param name="min">Inclusive</param>
	/// <param name="max">Exclusive</param>
	public float Range(float min, float max)
	{
		return ((float)_rand.NextDouble() * (max - min)) + min;         // System.Random.NextDouble() always returns greater than or 0.0f, or below 1.0f
	}

	/// <summary>
	/// </summary>
	/// <param name="minMax">minMax.x inclusive,  minMax.y exclusive.</param>
	public float Range(UnityEngine.Vector2 minMax)
	{
		return Range(minMax.x, minMax.y);
	}

	public bool RandBool()
	{
		return _rand.Next(0, 2) == 0;           //Test the disparity in a console app.
	}

	public bool NextBool()
	{
		return RandBool();
	}
}
