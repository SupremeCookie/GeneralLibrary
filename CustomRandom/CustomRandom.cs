//using Newtonsoft.Json;
using UnityEngine;

public class CustomRandom
{
	private System.Random rand;
	//[JsonProperty(PropertyName = "Seed")]
	private int seed;

	//[JsonIgnore]
	public int Seed { get { return seed; } }

	public CustomRandom()
	{
		seed = SeedGenerator.MathSeed;
		rand = new System.Random(seed);
	}

	public CustomRandom(int seed)
	{
		this.seed = seed;
		rand = new System.Random(this.seed);
	}

	public void OverwriteSeed(int seed)
	{
		this.seed = seed;
		rand = new System.Random(this.seed);
	}


	public int Next()
	{
		return rand.Next();
	}

	// Exlusive upper bound.
	public int Next(int maxValue)
	{
		return rand.Next(maxValue);
	}

	public double NextDouble()
	{
		return rand.NextDouble();              // System.Random.NextDouble() always returns greater than or 0.0f, or below 1.0f
	}

	/// <summary>
	/// Secretly uses NextDouble, then casts that to float
	/// Since it uses NextDouble, the range is between (inclusive) 0 and (exclusive) 1
	/// </summary>
	public float NextFloat()
	{
		return (float)rand.NextDouble();       // System.Random.NextDouble() always returns greater than or 0.0f, or below 1.0f
	}

	public void NextBytes(out byte[] byteBuffer)
	{
		byte[] buffer = new byte[] { };
		rand.NextBytes(buffer);

		byteBuffer = buffer;
	}

	public Vector2 NextVector(/*bool normalised = true*/) // Note DK: Current implementation automatically normalises.
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
		return rand.Next(min, max);
	}

	/// <summary>
	/// </summary>
	/// <param name="min">Inclusive</param>
	/// <param name="max">Exclusive</param>
	public float Range(float min, float max)
	{
		return ((float)rand.NextDouble() * (max - min)) + min;         // System.Random.NextDouble() always returns greater than or 0.0f, or below 1.0f
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
		return rand.Next(0, 2) == 0;
	}

	public bool NextBool()
	{
		return RandBool();
	}
}
