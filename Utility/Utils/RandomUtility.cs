
using UnityEngine;

public partial class Utility
{
	public static int GetWeightedRandom(in int min_incl, in int max_excl, in AnimationCurve curve, CustomRandom customRandom = null)
	{
		System.Diagnostics.Debug.Assert(curve != null, "RandomUtility -- AnimationCurve is null, please fix.");

		var result = -1;

		if (customRandom == null)
		{
			customRandom = CustomRandomContainer.GetRandom("RandomUtility");
		}


		int range = max_excl - min_incl;

		float totalSizeOfCurve = 0;
		float stepSize = 1.0f / range;
		float halfStepSize = stepSize * 0.5f;   // Note DK: We don't want to evaluate the steps at their beginnings, we want to evaluate right between beginning and end of each step. That way we get more acurate values.

		float[] weights = new float[range];

		for (int i = 0; i < range; ++i)
		{
			float progress = (i / (float)range) + halfStepSize;

			// Note DK: We store the starts of each region, so we can relate the final result back to a value in the given min-max range.
			weights[i] = totalSizeOfCurve;  // Note DK: Start of current region is the total progress to get TO this region.

			totalSizeOfCurve += curve.Evaluate(progress);
		}

		float randomValue = customRandom.Range(0f, totalSizeOfCurve);
		for (int i = 0; i < weights.Length; ++i)
		{
			float startOfRegion = weights[i];
			float endOfRegion = totalSizeOfCurve;
			if ((i + 1) < weights.Length)
			{
				endOfRegion = weights[i + 1];
			}

			bool liesInRegion = randomValue >= startOfRegion && randomValue < endOfRegion;
			if (liesInRegion)
			{
				result = i + min_incl;
				break;
			}
		}


		System.Diagnostics.Debug.Assert(result >= 0, "RandomUtility -- Resulting value is below 0, this means a valid value wasn't found. Please fix.");

		return result;
	}
}
