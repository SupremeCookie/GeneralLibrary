public static class DoubleExtensions
{
	public static float ConvertToFloat_ClampedToMaxValues(this double input)
	{
		float result = (float)input;

		if (float.IsPositiveInfinity(result))
		{
			result = float.MaxValue;
		}
		else if (float.IsNegativeInfinity(result))
		{
			result = float.MinValue;
		}

		return result;
	}
}
