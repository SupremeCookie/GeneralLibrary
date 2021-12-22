using UnityEngine;

[System.Serializable]
public class MinMax
{
	public Vector2 Min;
	public Vector2 Max;

	public float Width { get { return Max.x - Min.x; } }
	public float Height { get { return Max.y - Min.y; } }

	public MinMax()
	{
		Min = new Vector2(float.MaxValue, float.MaxValue);
		Max = new Vector2(float.MinValue, float.MinValue);
	}

	public MinMax(Vector2 min, Vector2 max)
	{
		Min = min;
		Max = max;
	}

	public MinMax(MinMax original)
	{
		Min = original.Min;
		Max = original.Max;
	}

	public Vector2 GetCenter()
	{
		return new Vector2()
		{
			x = Min.x + Width * 0.5f,
			y = Min.y + Height * 0.5f,
		};
	}

	public void TryAddNewMinOrMax(Vector2 proposedValue)
	{
		TryStoreNewMinOrMax(proposedValue);
	}

	public void TryStoreNewMinOrMax(Vector2 proposedValue)
	{
		if (proposedValue.x < Min.x)
		{
			Min.x = proposedValue.x;
		}

		if (proposedValue.y < Min.y)
		{
			Min.y = proposedValue.y;
		}

		if (proposedValue.x > Max.x)
		{
			Max.x = proposedValue.x;
		}

		if (proposedValue.y > Max.y)
		{
			Max.y = proposedValue.y;
		}
	}

	public override string ToString()
	{
		return string.Format("Min:({0}), Max({1})", Min, Max);
	}

	public static MinMax operator *(MinMax input, float multiplier)
	{
		var copyOfInput = new MinMax();
		copyOfInput.Min = input.Min;
		copyOfInput.Max = input.Max;

		var deltaX = copyOfInput.Max.x - copyOfInput.Min.x;
		var deltaY = copyOfInput.Max.y - copyOfInput.Min.y;

		var multipliedDelta = new Vector2(deltaX * multiplier, deltaY * multiplier);
		var halfMultipliedDelta = multipliedDelta * 0.5f;

		copyOfInput.Min.x += halfMultipliedDelta.x;
		copyOfInput.Min.y += halfMultipliedDelta.y;
		copyOfInput.Max.x -= halfMultipliedDelta.x;
		copyOfInput.Max.y -= halfMultipliedDelta.y;

		return copyOfInput;
	}
}