using UnityEngine;

[System.Serializable]
public class MinMaxRectangle
{
	public Vector2 min;
	public Vector2 max;

	public float width { get { return max.x - min.x; } }
	public float height { get { return max.y - min.y; } }
	public Vector2 size { get { return max - min; } }

	public MinMaxRectangle()
	{
		min = new Vector2(float.MaxValue, float.MaxValue);
		max = new Vector2(float.MinValue, float.MinValue);
	}

	public MinMaxRectangle(Vector2 min, Vector2 max)
	{
		this.min = min;
		this.max = max;
	}

	public MinMaxRectangle(MinMaxRectangle original)
	{
		min = original.min;
		max = original.max;
	}

	public Vector2 GetCenter()
	{
		return new Vector2()
		{
			x = min.x + width * 0.5f,
			y = min.y + height * 0.5f,
		};
	}

	public void TryAddNewMinOrMax(Vector2 proposedValue)
	{
		TryStoreNewMinOrMax(proposedValue);
	}

	public void TryStoreNewMinOrMax(Vector2 proposedValue)
	{
		if (proposedValue.x < min.x)
		{
			min.x = proposedValue.x;
		}

		if (proposedValue.y < min.y)
		{
			min.y = proposedValue.y;
		}

		if (proposedValue.x > max.x)
		{
			max.x = proposedValue.x;
		}

		if (proposedValue.y > max.y)
		{
			max.y = proposedValue.y;
		}
	}

	public override string ToString()
	{
		return string.Format("Min:({0}), Max({1})", min, max);
	}

	public static MinMaxRectangle operator *(MinMaxRectangle input, float multiplier)
	{
		var copyOfInput = new MinMaxRectangle();
		copyOfInput.min = input.min;
		copyOfInput.max = input.max;

		var deltaX = copyOfInput.max.x - copyOfInput.min.x;
		var deltaY = copyOfInput.max.y - copyOfInput.min.y;

		var multipliedDelta = new Vector2(deltaX * multiplier, deltaY * multiplier);
		var halfMultipliedDelta = multipliedDelta * 0.5f;

		copyOfInput.min.x += halfMultipliedDelta.x;
		copyOfInput.min.y += halfMultipliedDelta.y;
		copyOfInput.max.x -= halfMultipliedDelta.x;
		copyOfInput.max.y -= halfMultipliedDelta.y;

		return copyOfInput;
	}
}