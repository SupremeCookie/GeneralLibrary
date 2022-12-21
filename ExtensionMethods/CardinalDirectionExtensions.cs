using UnityEngine;

public static class CardinalDirectionExtensions
{
	private static CardinalDirectionalVector[] _octagonalDirectionArray = new CardinalDirectionalVector[]
	{
		new CardinalDirectionalVector{Vector = new Vector2(-1, 0), Direction = CARDINAL_DIRECTION.Left },
		new CardinalDirectionalVector{Vector = new Vector2(0, 1), Direction = CARDINAL_DIRECTION.Up},
		new CardinalDirectionalVector{Vector = new Vector2(1, 0), Direction = CARDINAL_DIRECTION.Right},
		new CardinalDirectionalVector{Vector = new Vector2(0, -1), Direction = CARDINAL_DIRECTION.Down},
	};

	public static Vector2 ToVector2(this CARDINAL_DIRECTION direction)
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

	public static CARDINAL_DIRECTION ToCardinalDirection(this Vector2 dir)
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

	public static bool IsLeft(this CARDINAL_DIRECTION dir)
	{
		return dir == CARDINAL_DIRECTION.Left;
	}

	public static CARDINAL_DIRECTION Flip(this CARDINAL_DIRECTION dir)
	{
		var vector = dir.ToVector2();
		vector *= -1f;
		var newDir = vector.ToCardinalDirection();

		return newDir;
	}
}
