using System;
using UnityEngine;

[Serializable]
public class MinMaxCurves
{
	[Readonly] public float ValueScalar = 10.0f;

	public AnimationCurve Min;
	public AnimationCurve Max;

	public Vector2 Evaluate(float time)
	{
		return new Vector2
		{
			x = Min.Evaluate(time),
			y = Max.Evaluate(time),
		} * ValueScalar;
	}
}