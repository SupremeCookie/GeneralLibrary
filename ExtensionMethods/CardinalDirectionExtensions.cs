using UnityEngine;

public static class CardinalDirectionExtensions
{
	private static CardinalDirectionalVector[] _octagonalDirectionArray = new CardinalDirectionalVector[]
	{
		new CardinalDirectionalVector{Vector = new Vector2(-1, 0), Direction = CardinalDirection.Left },
		new CardinalDirectionalVector{Vector = new Vector2(0, 1), Direction = CardinalDirection.Up},
		new CardinalDirectionalVector{Vector = new Vector2(1, 0), Direction = CardinalDirection.Right},
		new CardinalDirectionalVector{Vector = new Vector2(0, -1), Direction = CardinalDirection.Down},
	};

	public static Vector2 ToVector2(this CardinalDirection direction)
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

	public static CardinalDirection ToCardinalDirection(this Vector2 dir)
	{
		float highestDot = 0f;
		int index = -1;
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

	public static bool IsLeft(this CardinalDirection dir)
	{
		return dir == CardinalDirection.Left;
	}

	public static CardinalDirection Flip(this CardinalDirection dir)
	{
		var vector = dir.ToVector2();
		vector *= -1f;
		var newDir = vector.ToCardinalDirection();

		return newDir;
	}

	public static CardinalDirection[] GetPerpendiculars(this CardinalDirection dir)
	{
		var result = new CardinalDirection[2];

		switch (dir)
		{
			case CardinalDirection.Down:
			case CardinalDirection.Up:
			{
				result[0] = CardinalDirection.Left;
				result[1] = CardinalDirection.Right;
				break;
			}

			case CardinalDirection.Left:
			case CardinalDirection.Right:
			{
				result[0] = CardinalDirection.Up;
				result[1] = CardinalDirection.Down;
				break;
			}

			case CardinalDirection.None:
			{
				Debug.LogError($"No perpendicular direction can be made for direction: {CardinalDirection.None}");
				break;
			}
		}

		return result;
	}
}
