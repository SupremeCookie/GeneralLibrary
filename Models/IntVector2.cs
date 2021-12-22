using UnityEngine;

[System.Serializable]
public class IntVector2
{
	public int x;
	public int y;

	public float magnitude { get { return Mathf.Sqrt((x * x) + (y * y)); } }

	public IntVector2() { }
	public IntVector2(int x, int y) { this.x = x; this.y = y; }

	public override string ToString()
	{
		return string.Format("{0}, {1}", x, y);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static IntVector2 operator *(IntVector2 first, int multiplier)
	{
		var returnValue = new IntVector2();
		returnValue.x = first.x * multiplier;
		returnValue.y = first.y * multiplier;
		return returnValue;
	}

	public static IntVector2 operator *(IntVector2 first, float multiplier)
	{
		var returnValue = new IntVector2();
		returnValue.x = Mathf.RoundToInt(first.x * multiplier);
		returnValue.y = Mathf.RoundToInt(first.y * multiplier);
		return returnValue;
	}

	public static Vector2 operator *(IntVector2 first, Vector2 multiplier)
	{
		return new Vector2
		{
			x = first.x * multiplier.x,
			y = first.y * multiplier.y,
		};
	}

	public static Vector2 operator *(Vector2 first, IntVector2 multiplier)
	{
		return new Vector2
		{
			x = first.x * multiplier.x,
			y = first.y * multiplier.y,
		};
	}

	public static IntVector2 operator -(IntVector2 first, IntVector2 second)
	{
		return new IntVector2(first.x - second.x, first.y - second.y);
	}

	public static IntVector2 operator +(IntVector2 first, IntVector2 second)
	{
		return new IntVector2(first.x + second.x, first.y + second.y);
	}

	public static bool operator ==(IntVector2 first, IntVector2 second)
	{
		if (ReferenceEquals(first, second))
			return true;

		if (ReferenceEquals(first, null))
			return false;

		if (ReferenceEquals(second, null))
			return false;

		return first.x == second.x && first.y == second.y;
	}

	public override bool Equals(object obj)
	{
		return this == (IntVector2)obj;
	}

	public static bool operator !=(IntVector2 first, IntVector2 second)
	{
		return !(first == second);
	}




	public static implicit operator Vector3(IntVector2 pos)
	{
		return new Vector3(pos.x, 0, pos.y);
	}

	public static implicit operator IntVector2(Vector2 pos)
	{
		return new IntVector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
	}
}