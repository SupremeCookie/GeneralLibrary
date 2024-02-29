using UnityEngine;

[System.Serializable]
public class Coordinates
{
	public int x;
	public int y;

	public float sqrMagnitude
	{
		get
		{
			return (x * x) + (y * y);
		}
	}


	public Coordinates() { }
	public Coordinates(int x, int y) { this.x = x; this.y = y; }
	public Coordinates(float x, float y) { this.x = Utility.RoundToNearestIntOrCeil(x); this.y = Utility.RoundToNearestIntOrCeil(y); }
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
}