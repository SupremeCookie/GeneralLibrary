using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
	private static CustomRandom _rand;
	private static CustomRandom _random
	{
		get
		{
			if (_rand == null)
			{
				_rand = new CustomRandom();
			}

			return _rand;
		}
	}

	#region VEC 3
	public static Vector3 MultiplyByVec3(this Vector3 vec, Vector3 mult)
	{
		Vector3 newVec = new Vector3(vec.x, vec.y, vec.z);
		newVec.x *= mult.x;
		newVec.y *= mult.y;
		newVec.z *= mult.z;
		return newVec;
	}

	public static Vector3 MultiplyByVec3(this Vector3 vec, float x, float y, float z)
	{
		Vector3 newVec = new Vector3(vec.x, vec.y, vec.z);
		newVec.x *= x;
		newVec.y *= y;
		newVec.z *= z;
		return newVec;
	}

	public static Vector2 DivideByVec3(this Vector3 vec, Vector3 div)
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
		if (newVec.x < 0)
		{
			newVec.x = Mathf.Abs(newVec.x);
		}

		if (newVec.y < 0)
		{
			newVec.y = Mathf.Abs(newVec.y);
		}

		if (newVec.z < 0)
		{
			newVec.z = Mathf.Abs(newVec.z);
		}

		return newVec;
	}

	public static Vector3 Random(this Vector3 vec)
	{
		vec.x = _random.Range(0f, 1f);
		vec.y = _random.Range(0f, 1f);
		vec.z = _random.Range(0f, 1f);

		vec.Normalize();

		return vec;
	}

	public static bool IsValid(this Vector3 vec)
	{
		bool isPositiveInfi = vec.x == float.PositiveInfinity || vec.y == float.PositiveInfinity || vec.z == float.PositiveInfinity;
		bool isNegativeInfi = vec.x == float.NegativeInfinity || vec.y == float.NegativeInfinity || vec.z == float.NegativeInfinity;
		bool isNan = vec.x == float.NaN || vec.y == float.NaN || vec.z == float.NaN;

		return !isPositiveInfi && !isNegativeInfi && !isNan;
	}
	#endregion

	#region CONVERTERS
	public static Vector3 XYVectorToXZ(this Vector2 vec)
	{
		return new Vector3(vec.x, 0, vec.y);
	}

	public static Vector2 XZVectorToXY(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.z);
	}
	#endregion

	#region OPERATORS
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
	#endregion

	#region VEC 2
	public static Vector2Int ToVector2Int(this Vector2 input)
	{
		return new Vector2Int(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));
	}

	public static Vector2 MultiplyByVec2(this Vector2 vec, Vector2 mult)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec.x *= mult.x;
		newVec.y *= mult.y;
		return newVec;
	}

	public static Vector2 MultiplyByVec2(this Vector2 vec, float x, float y)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec.x *= x;
		newVec.y *= y;
		return newVec;
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

	public static Vector2 ScaleRandomly(this Vector2 vec, Vector2 range)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec.x *= _random.Range(range.x, range.y);
		newVec.y *= _random.Range(range.x, range.y);
		return newVec;
	}

	public static Vector2 ScaleUniformly(this Vector2 vec, Vector2 range)
	{
		Vector2 newVec = new Vector2(vec.x, vec.y);
		newVec *= _random.Range(range.x, range.y);
		return newVec;
	}

	public static Vector2 RoundToInt(this Vector2 vec)
	{
		var newVec = new Vector2();
		newVec.x = Mathf.RoundToInt(vec.x);
		newVec.y = Mathf.RoundToInt(vec.y);
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

	public static Vector2 Random(this Vector2 vec)
	{
		vec = _random.NextVector();
		return vec;
	}

	public static bool IsBiggerThan(this Vector2 first, Vector2 second)
	{
		return first.x > second.x
			&& first.y > second.y;
	}

	public static bool IsSmallerThan(this Vector2 first, Vector2 second)
	{
		return first.x < second.x
			&& first.y < second.y;
	}

	public static bool CardinallyAlligns(this Vector2 first, Vector2 second)
	{
		bool xAlligns = first.x.IsCloseTo(second.x);
		bool yAlligns = first.y.IsCloseTo(second.y);

		return xAlligns ^ yAlligns;
	}

	public static bool IsValid(this Vector2 vec)
	{
		bool isPositiveInfi = vec.x == float.PositiveInfinity || vec.y == float.PositiveInfinity;
		bool isNegativeInfi = vec.x == float.NegativeInfinity || vec.y == float.NegativeInfinity;
		bool isNan = vec.x == float.NaN || vec.y == float.NaN;

		return !isPositiveInfi && !isNegativeInfi && !isNan;
	}

	public static List<Vector2> Copy(this List<Vector2> input)
	{
		var result = new List<Vector2>(input.Count);

		for (int i = 0; i < input.Count; ++i)
		{
			result.Add(input[i]);
		}

		return result;
	}
	#endregion
}
