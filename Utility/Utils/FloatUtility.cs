﻿using UnityEngine;

public partial class Utility
{
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

	public static float Lerp(float min, float max, float percentage)
	{
		float range = max - min;
		float valueOnRange = range * percentage;
		float result = min + valueOnRange;

		// shorthand:   min + ((max - min) * range);

		return result;
	}

	public static float ConvertToDB(float volume, float volumeBase)
	{
		float dbVolume = -80f;

		// We re-map the linear scale we have to the logarithmic one of db, which goes as follows.
		// Can't log10 a 0 value.
		if (volume > 0)
		{
			float relativeVolume = volume / volumeBase;
			dbVolume = 20 * Mathf.Log10(relativeVolume);
		}

		return dbVolume;
	}

	public static float ConvertFromDB(float volume, float volumeBase)
	{
		float preLogValue = volume / 20;
		float reLogged = Mathf.Pow(10, preLogValue);

		return volumeBase * reLogged;
	}

	public static float Dot(Vector2 g, float x, float y)
	{
		return g.x * x + g.y * y;
	}

	public static float Dot(Vector3 g, float x, float y, float z)
	{
		return g.x * x + g.y * y + g.z * z;
	}
}
