using UnityEngine;

public static class CardinalDirectionExtensions
{
	private static CardinalDirectionalVector[] cardinalDirectionArray = new CardinalDirectionalVector[]
	{
		new CardinalDirectionalVector { vector = new Vector2(-1, 0), direction = CardinalDirection.Left },
		new CardinalDirectionalVector { vector = new Vector2(0, 1), direction = CardinalDirection.Up },
		new CardinalDirectionalVector { vector = new Vector2(1, 0), direction = CardinalDirection.Right },
		new CardinalDirectionalVector { vector = new Vector2(0, -1), direction = CardinalDirection.Down },
	};

	public static Vector2 ToVector2(this CardinalDirection direction)
	{
		Vector2 result = Vector2.zero;

		for (int i = 0; i < cardinalDirectionArray.Length; ++i)
		{
			if (cardinalDirectionArray[i].direction == direction)
			{
				result = cardinalDirectionArray[i].vector;
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

		for (int i = 0; i < cardinalDirectionArray.Length; ++i)
		{
			var dotProd = DotProduct.CalculateDotProduct(inputNormalized, cardinalDirectionArray[i].vector);

			if (dotProd > highestDot)
			{
				index = i;
				highestDot = dotProd;
			}
		}

		Debug.Assert(index >= 0, "Index is not equal or greater than 0, this shouldn't happen");
		return cardinalDirectionArray[index].direction;
	}

	public static bool IsLeft(this CardinalDirection dir)
	{
		return dir == CardinalDirection.Left;
	}

	public static CardinalDirection Flip(this CardinalDirection dir)
	{
		CardinalDirection newDir = dir;
		switch (dir)
		{
			case CardinalDirection.Left: { newDir = CardinalDirection.Right; break; }
			case CardinalDirection.Right: { newDir = CardinalDirection.Left; break; }
			case CardinalDirection.Up: { newDir = CardinalDirection.Down; break; }
			case CardinalDirection.Down: { newDir = CardinalDirection.Up; break; }
		}

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
