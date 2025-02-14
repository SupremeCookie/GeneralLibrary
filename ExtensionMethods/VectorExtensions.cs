using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
	private static CustomRandom rand;
	private static CustomRandom random
	{
		get
		{
			if (rand == null)
			{
				rand = CustomRandomContainer.GetRandomInstance("Vector_Extensions");
			}

			return rand;
		}
	}


	public static Vector2 ToVector2(this Vector3 input)
	{
		return (Vector2)input;
	}

	public static Vector2 ToVector2(this Vector2 input)
	{
		return input;
	}

	public static Vector3 ToVector3(this Vector3 input)
	{
		return input;
	}

	public static Vector3 ToVector3(this Vector2 input)
	{
		return (Vector3)input;
	}


	public static Vector3 MultiplyByVec3(this Vector3 vec, Vector3 mult)
	{
		mult.x *= vec.x;
		mult.y *= vec.y;
		mult.z *= vec.z;
		return mult;
	}

	public static Vector3 MultiplyByVec3(this Vector3 vec, float x, float y, float z)
	{
		return vec.MultiplyByVec3(new Vector3(x, y, z));
	}

	public static Vector3 DivideByVec3(this Vector3 vec, Vector3 div)
	{
		Vector3 newVec = new Vector3(vec.x, vec.y, vec.z);
		newVec.x /= div.x;
		newVec.y /= div.y;
		newVec.z /= div.z;
		return newVec;
	}

	public static Vector3 Abs(this Vector3 vec)
	{
		Vector3 newVec = new Vector3(vec.x, vec.y, vec.z);

		newVec.x = Mathf.Abs(newVec.x);
		newVec.y = Mathf.Abs(newVec.y);
		newVec.z = Mathf.Abs(newVec.z);

		return newVec;
	}


	public static Vector3 XYVectorToXZ(this Vector2 vec)
	{
		return new Vector3(vec.x, 0, vec.y);
	}

	public static Vector2 XZVectorToXY(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.z);
	}


	public static bool EqualTo(this Vector3 first, Vector3 second)
	{
		return (first.x.IsCloseTo(second.x))
			&& (first.y.IsCloseTo(second.y))
			&& (first.z.IsCloseTo(second.z));
	}

	public static bool IsCloseTo(this Vector3 first, Vector3 second)
	{
		return (first.x.IsCloseTo(second.x))
			&& (first.y.IsCloseTo(second.y))
			&& (first.z.IsCloseTo(second.z));
	}

	public static bool IsCloseTo(this Vector3 first, Vector3 second, float maxDistance)
	{
		return (first.x.IsCloseTo(second.x, maxDistance))
			&& (first.y.IsCloseTo(second.y, maxDistance))
			&& (first.z.IsCloseTo(second.z, maxDistance));
	}

	public static bool EqualTo(this Vector2 first, Vector2 second)
	{
		return (first.x.IsCloseTo(second.x))
			&& (first.y.IsCloseTo(second.y));
	}

	public static bool IsCloseTo(this Vector2 first, Vector2 second)
	{
		return (first.x.IsCloseTo(second.x))
			&& (first.y.IsCloseTo(second.y));
	}

	public static bool IsCloseTo(this Vector2 first, Vector2 second, float maxDistance)
	{
		return (first.x.IsCloseTo(second.x, maxDistance))
			&& (first.y.IsCloseTo(second.y, maxDistance));
	}


	public static Vector2Int ToVector2Int(this Vector2 input)
	{
		return new Vector2Int((int)input.x.RoundToNearest(1), (int)input.y.RoundToNearest(1));
	}

	public static Vector2 MultiplyByVec2(this Vector2 vec, Vector2 mult)
	{
		mult.x *= vec.x;
		mult.y *= vec.y;
		return mult;
	}

	public static Vector2 MultiplyByVec2(this Vector2 vec, float x, float y)
	{
		return vec.MultiplyByVec2(new Vector2(x, y));
	}

	public static Vector2 DivideByVec2(this Vector2 vec, Vector2 div)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec.x /= div.x;
		newVec.y /= div.y;
		return newVec;
	}

	public static Vector2 RoundToNearest(this Vector2 vec, float roundTo)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec.x = vec.x.RoundToNearest(roundTo);
		newVec.y = vec.y.RoundToNearest(roundTo);
		return newVec;
	}

	public static Vector2 CeilToNearest(this Vector2 vec)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec.x = Mathf.Ceil(vec.x);
		newVec.y = Mathf.Ceil(vec.y);
		return newVec;
	}

	public static Vector2 RoundToInt(this Vector2 vec)
	{
		var newVec = new Vector2();
		newVec.x = vec.x.RoundToNearest(1);
		newVec.y = vec.y.RoundToNearest(1);
		return newVec;
	}

	public static Vector2 RotateVectorAroundVector(this Vector2 target, Vector2 around, float angleInDegrees)
	{
		var deltaVector = target - around;
		var magnitudeOfDelta = deltaVector.magnitude;

		return target.RotateVectorAroundVector(around, angleInDegrees, magnitudeOfDelta);
	}

	public static Vector2 RotateVectorAroundVector(this Vector2 target, Vector2 around, float angleInDegrees, float radius)
	{
		var deltaVector = target - around;
		var normalisedDelta = deltaVector.normalized;

		var rotatedVector = Matrices.ApplyRotationMatrix(normalisedDelta, angleInDegrees);

		var magnitudedRotatedVector = rotatedVector * radius;

		var resultVector = around + magnitudedRotatedVector;

		return resultVector;
	}

	public static bool IsCardinallyAlligned(this Vector2 first, Vector2 second)
	{
		bool xAlligns = first.x.IsCloseTo(second.x);
		bool yAlligns = first.y.IsCloseTo(second.y);

		return xAlligns ^ yAlligns;
	}
}
