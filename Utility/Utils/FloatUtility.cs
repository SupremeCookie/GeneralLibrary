using UnityEngine;

public partial class Utility
{
	public static bool FloatsOppositeSigns(float first, float second)
	{
		return ((first * second) >= 0.0f);
	}

	public static float InverseLerp(float value, float min, float max)
	{
		if (max < min)
		{
			Debug.LogWarning($"Max ({max}) is smaller than Min ({min}), returning 0");
			return 0f;
		}

		if (value < min)
		{
			return 0f;
		}

		if (value > max)
		{
			return 1f;
		}

		float minToMax = max - min;
		float minToValue = value - min;

		return Mathf.Clamp01(minToValue / minToMax);
	}
}
