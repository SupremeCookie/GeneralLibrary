using System.Linq;
using UnityEngine;

public static class CoordinatesExtensions
{
	// =================================
	// Coordinates
	// =================================
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

	// =================================
	// Coordinates 3D
	// =================================
	public static void TryStoreNewMin(this Coordinates3D input, Coordinates3D suggestedValue)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		input.x = Mathf.Min(input.x, suggestedValue.x);
		input.y = Mathf.Min(input.y, suggestedValue.y);
		input.z = Mathf.Min(input.z, suggestedValue.z);
	}

	public static void TryStoreNewMax(this Coordinates3D input, Coordinates3D suggestedValue)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		input.x = Mathf.Max(input.x, suggestedValue.x);
		input.y = Mathf.Max(input.y, suggestedValue.y);
		input.z = Mathf.Max(input.z, suggestedValue.z);
	}

	public static Vector2 ToVector2(this Coordinates3D input)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		return new Vector2(input.x, input.y);
	}

	public static Vector3 ToVector3(this Coordinates3D input)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		return new Vector3(input.x, input.y, input.z);
	}

	public static void SetValueTo(this Coordinates3D input, Vector2 value)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		input.x = Utility.RoundToNearestIntOrCeil(value.x);
		input.y = Utility.RoundToNearestIntOrCeil(value.y);

		input.z = 0;
	}

	public static void SetValueTo(this Coordinates3D input, Vector3 value)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		input.x = Utility.RoundToNearestIntOrCeil(value.x);
		input.y = Utility.RoundToNearestIntOrCeil(value.y);
		input.z = Utility.RoundToNearestIntOrCeil(value.z);
	}


	public static void AddOffset(this Coordinates3D input, Coordinates3D offset)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		input.x += offset.x;
		input.y += offset.y;
		input.z += offset.z;
	}


	public static bool IsNotZero(this Coordinates3D input)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		return input.x != 0 || input.y != 0 || input.z != 0;
	}


	public static int GetCoordinateHash(int x, int y, int z)
	{
		int hash = 19;
		hash = hash * 233 + x.GetHashCode();
		hash = hash * 2777 + y.GetHashCode();
		hash = hash * 7103 + z.GetHashCode();
		return hash;
	}

	public static int GetCoordinateHash(this Coordinates3D input)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		return GetCoordinateHash(input.x, input.y, input.z);
	}

	public static int ToHash(this Coordinates3D input)
	{
		Debug.Assert(input != null, $"{typeof(Coordinates3D).ToString()} input value is null, can't execute the extension method");
		return input.GetCoordinateHash();
	}

	public static Coordinates3D Copy(this Coordinates3D input)
	{
		return new Coordinates3D(input.x, input.y, input.z);
	}
}
