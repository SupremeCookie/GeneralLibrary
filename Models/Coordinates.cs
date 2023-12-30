using UnityEngine;

[System.Serializable]
public class Coordinates
{
	public int x;
	public int y;

	public Coordinates() { }
	public Coordinates(int x, int y) { this.x = x; this.y = y; }
	public Coordinates(Vector2 input) { this.x = Utility.RoundToNearestIntOrCeil(input.x); this.y = Utility.RoundToNearestIntOrCeil(input.y); }

	public override int GetHashCode()
	{
		return this.GetCoordinateHash();
	}

	public override bool Equals(object obj)
	{
		return GetHashCode().Equals(obj.GetHashCode());
	}

	public override string ToString()
	{
		return $"({x}, {y})";
	}


	public static Coordinates operator -(Coordinates first, Coordinates second)
	{
		Coordinates result = new Coordinates(first.x - second.x, first.y - second.y);
		return result;
	}

	public static Coordinates operator +(Coordinates first, Coordinates second)
	{
		Coordinates result = new Coordinates(first.x + second.x, first.y + second.y);
		return result;
	}

	public float sqrMagnitude
	{
		get
		{
			return (x * x) + (y * y);
		}
	}
}

public static class CoordinatesExtensions
{
	public static void TryStoreNewMin(this Coordinates input, Coordinates suggestedValue)
	{
		input.x = Mathf.Min(input.x, suggestedValue.x);
		input.y = Mathf.Min(input.y, suggestedValue.y);
	}

	public static void TryStoreNewMax(this Coordinates input, Coordinates suggestedValue)
	{
		input.x = Mathf.Max(input.x, suggestedValue.x);
		input.y = Mathf.Max(input.y, suggestedValue.y);
	}

	public static Vector2 ToVector2(this Coordinates input)
	{
		return new Vector2(input.x, input.y);
	}

	public static Vector3 ToVector3(this Coordinates input)
	{
		return new Vector3(input.x, input.y, 0);
	}

	public static int GetCoordinateHash(int x, int y)
	{
		x += 1000;
		y += 1000;

		int temp = y + ((x + 1) / 2);
		return x + (temp * temp);
	}

	public static int GetCoordinateHash(this Coordinates input)
	{
		return GetCoordinateHash(input.x, input.y);
	}

	public static int ToHash(this Coordinates input)
	{
		return input.GetCoordinateHash();
	}
}