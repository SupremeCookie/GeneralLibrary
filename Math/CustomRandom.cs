public class CustomRandom
{
	private System.Random _rand;

	public CustomRandom()
	{
		_rand = new System.Random(SeedGenerator.MathSeed);
	}

	public CustomRandom(int seed)
	{
		_rand = new System.Random(seed);
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
		return _rand.NextDouble();
	}

	public void NextBytes(out byte[] byteBuffer)
	{
		byte[] buffer = new byte[] { };
		_rand.NextBytes(buffer);

		byteBuffer = buffer;
	}

	public int Range(int min, int max)
	{
		return _rand.Next(min, max);
	}

	public float Range(float min, float max)
	{
		return ((float)_rand.NextDouble() * (max - min)) + min;
	}

	public float Range(UnityEngine.Vector2 minMax)
	{
		return Range(minMax.x, minMax.y);
	}

	public bool RandBool()
	{
		return _rand.Next(0, 2) == 0;   //Test the disparity in a console app.
	}
}
