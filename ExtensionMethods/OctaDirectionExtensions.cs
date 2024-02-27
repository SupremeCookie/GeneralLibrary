using RogueLike;
using UnityEngine;

public static class OctaDirectionExtensions
{
	private static DirectionalVector[] _octagonalDirectionArray = new DirectionalVector[]
	{
		new DirectionalVector{Vector = new Vector2(-1, 0), Direction = OctaDirection.Left },
		new DirectionalVector{Vector = new Vector2(0, 1), Direction = OctaDirection.Up},
		new DirectionalVector{Vector = new Vector2(1, 0), Direction = OctaDirection.Right},
		new DirectionalVector{Vector = new Vector2(0, -1), Direction = OctaDirection.Down},
		new DirectionalVector{Vector = new Vector2(-0.5f, 0.5f).normalized, Direction = OctaDirection.UpLeft},
		new DirectionalVector{Vector = new Vector2(-0.5f, -0.5f).normalized, Direction = OctaDirection.DownLeft},
		new DirectionalVector{Vector = new Vector2(0.5f, 0.5f).normalized, Direction = OctaDirection.UpRight},
		new DirectionalVector{Vector = new Vector2(0.5f, -0.5f).normalized, Direction = OctaDirection.DownRight},
	};

	public static Vector2 ToVector2(this OctaDirection direction)
	{
		Vector2 result = Vector2.zero;

		for (int i = 0; i < _octagonalDirectionArray.Length; ++i)
		{
			if (_octagonalDirectionArray[i].Direction == direction)
			{
				result = _octagonalDirectionArray[i].Vector;
				break;
			}
		}

		return result;
	}

	public static OctaDirection ToOctaDirection(this Vector2 dir)
	{
		float highestDot = 0f;
		int index = 0;
		Vector2 inputNormalized = dir.normalized;

		for (int i = 0; i < _octagonalDirectionArray.Length; ++i)
		{
			var dotProd = DotProduct.CalculateDotProduct(inputNormalized, _octagonalDirectionArray[i].Vector);

			if (dotProd > highestDot)
			{
				index = i;
				highestDot = dotProd;
			}
		}

		return _octagonalDirectionArray[index].Direction;
	}

	public static bool IsLeft(this OctaDirection dir)
	{
		return dir == OctaDirection.DownLeft || dir == OctaDirection.Left || dir == OctaDirection.UpLeft;
	}

	public static OctaDirection Flip(this OctaDirection dir)
	{
		var vector = dir.ToVector2();
		vector *= -1f;
		var newDir = vector.ToOctaDirection();

		return newDir;
	}

	public static OctaDirection Inverse(this OctaDirection dir)
	{
		return dir.Flip();
	}
}