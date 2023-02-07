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

	public static CARDINAL_DIRECTION[] GetPerpendiculars(this CARDINAL_DIRECTION dir)
	{
		var result = new CARDINAL_DIRECTION[2];

		switch (dir)
		{
			case CARDINAL_DIRECTION.Down:
			case CARDINAL_DIRECTION.Up:
			{
				result[0] = CARDINAL_DIRECTION.Left;
				result[1] = CARDINAL_DIRECTION.Right;
				break;
			}

			case CARDINAL_DIRECTION.Left:
			case CARDINAL_DIRECTION.Right:
			{
				result[0] = CARDINAL_DIRECTION.Up;
				result[1] = CARDINAL_DIRECTION.Down;
				break;
			}

			case CARDINAL_DIRECTION.None:
			{
				Debug.LogError($"No perpendicular direction can be made for direction: {CARDINAL_DIRECTION.None}");
				break;
			}
		}

		return result;
	}
}
