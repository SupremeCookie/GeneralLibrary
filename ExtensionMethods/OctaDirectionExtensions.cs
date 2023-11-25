using RogueLike;
using UnityEngine;

public static class OctaDirectionExtensions
{
	private static DirectionalVector[] _octagonalDirectionArray = new DirectionalVector[]
	{
		new DirectionalVector{Vector = new Vector2(-1, 0), Direction = OCTA_DIRECTION.Left },
		new DirectionalVector{Vector = new Vector2(0, 1), Direction = OCTA_DIRECTION.Up},
		new DirectionalVector{Vector = new Vector2(1, 0), Direction = OCTA_DIRECTION.Right},
		new DirectionalVector{Vector = new Vector2(0, -1), Direction = OCTA_DIRECTION.Down},
		new DirectionalVector{Vector = new Vector2(-0.5f, 0.5f).normalized, Direction = OCTA_DIRECTION.UpLeft},
		new DirectionalVector{Vector = new Vector2(-0.5f, -0.5f).normalized, Direction = OCTA_DIRECTION.DownLeft},
		new DirectionalVector{Vector = new Vector2(0.5f, 0.5f).normalized, Direction = OCTA_DIRECTION.UpRight},
		new DirectionalVector{Vector = new Vector2(0.5f, -0.5f).normalized, Direction = OCTA_DIRECTION.DownRight},
	};

	public static Vector2 ToVector2(this OCTA_DIRECTION direction)
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

	public static OCTA_DIRECTION ToOctaDirection(this Vector2 dir)
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

	public static bool IsLeft(this OCTA_DIRECTION dir)
	{
		return dir == OCTA_DIRECTION.DownLeft || dir == OCTA_DIRECTION.Left || dir == OCTA_DIRECTION.UpLeft;
	}

	public static OCTA_DIRECTION Flip(this OCTA_DIRECTION dir)
	{
		var vector = dir.ToVector2();
		vector *= -1f;
		var newDir = vector.ToOctaDirection();

		return newDir;
	}

	public static OCTA_DIRECTION Inverse(this OCTA_DIRECTION dir)
	{
		return dir.Flip();
	}
}