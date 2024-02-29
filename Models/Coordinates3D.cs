using UnityEngine;

[System.Serializable]
public class Coordinates3D
{
	public int x;
	public int y;
	public int z;

	public Coordinates3D() { }
	public Coordinates3D(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }
	public Coordinates3D(float x, float y, float z) { this.x = Utility.RoundToNearestIntOrCeil(x); this.y = Utility.RoundToNearestIntOrCeil(y); this.z = Utility.RoundToNearestIntOrCeil(z); }
	public Coordinates3D(Vector3 xyz)
	{
#if MM
		float cubeHeight = DebugData.Instance.cubeHeight;
#else
		float cubeHeight = 1.0f;
#endif

		this.x = Utility.RoundToNearestIntOrCeil(xyz.x);
		this.y = Utility.RoundToNearestIntOrCeil(xyz.y / cubeHeight);
		this.z = Utility.RoundToNearestIntOrCeil(xyz.z);
	}

	public override int GetHashCode()
	{
		return this.GetCoordinateHash();
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, this) || object.ReferenceEquals(null, obj))
		{
			return false;
		}

		return GetHashCode().Equals(obj.GetHashCode());
	}

	public override string ToString()
	{
		return $"({x}, {y}, {z})";
	}



	public static bool operator ==(Coordinates3D first, Coordinates3D second)
	{
		if (object.ReferenceEquals(null, first) || object.ReferenceEquals(null, second))
		{
			return false;
		}

		return first.GetHashCode().Equals(second.GetHashCode());
	}

	public static bool operator !=(Coordinates3D first, Coordinates3D second)
	{
		bool firstObjectIsNull = object.ReferenceEquals(null, first);
		bool secondObjectIsNull = object.ReferenceEquals(null, second);
		if (firstObjectIsNull && secondObjectIsNull)
		{
			return false;
		}

		if ((firstObjectIsNull && !secondObjectIsNull) || (!firstObjectIsNull && secondObjectIsNull))
		{
			return true;
		}

		return first.GetHashCode().Equals(second.GetHashCode());
	}

	public static Coordinates3D operator +(Coordinates3D first, Coordinates3D second)
	{
		return new Coordinates3D(first.x + second.x, first.y + second.y, first.z + second.z);
	}

	public static Coordinates3D operator -(Coordinates3D first, Coordinates3D second)
	{
		return new Coordinates3D(first.x - second.x, first.y - second.y, first.z - second.z);
	}

	public static Coordinates3D operator *(Coordinates3D first, int scalar)
	{
		return new Coordinates3D(first.x * scalar, first.y * scalar, first.z * scalar);
	}
}