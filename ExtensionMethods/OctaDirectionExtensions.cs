using RogueLike;
using UnityEngine;

public static class OctaDirectionExtensions
{
	private static DirectionVector[] _octagonalDirectionArray = new DirectionVector[]
	{
		new DirectionVector { vector = new Vector2(-1, 0), direction = OctaDirection.Left },
		new DirectionVector { vector = new Vector2(0, 1), direction = OctaDirection.Up },
		new DirectionVector { vector = new Vector2(1, 0), direction = OctaDirection.Right },
		new DirectionVector { vector = new Vector2(0, -1), direction = OctaDirection.Down },
		new DirectionVector { vector = new Vector2(-0.5f, 0.5f).normalized, direction = OctaDirection.UpLeft },
		new DirectionVector { vector = new Vector2(-0.5f, -0.5f).normalized, direction = OctaDirection.DownLeft },
		new DirectionVector { vector = new Vector2(0.5f, 0.5f).normalized, direction = OctaDirection.UpRight },
		new DirectionVector { vector = new Vector2(0.5f, -0.5f).normalized, direction = OctaDirection.DownRight },
	};

	public static Vector2 ToVector2(this OctaDirection direction)
	{
		Vector2 result = Vector2.zero;

		for (int i = 0; i < _octagonalDirectionArray.Length; ++i)
		{
			if (_octagonalDirectionArray[i].direction == direction)
			{
				result = _octagonalDirectionArray[i].vector;
				break;
			}
		}

		return result;
	}

	public static OctaDirection ToOctaDirection(this Vector2 dir)
	{
		float highestDot = 0f;
		int index = -1;
		Vector2 inputNormalized = dir.normalized;

		for (int i = 0; i < _octagonalDirectionArray.Length; ++i)
		{
			var dotProd = DotProduct.CalculateDotProduct(inputNormalized, _octagonalDirectionArray[i].vector);

			if (dotProd > highestDot)
			{
				index = i;
				highestDot = dotProd;
			}
		}

		Debug.Assert(index >= 0, $"Calculated index is invalid, please fix");

		return _octagonalDirectionArray[index].direction;
	}

	public static bool IsLeft(this OctaDirection dir)
	{
		return dir == OctaDirection.DownLeft || dir == OctaDirection.Left || dir == OctaDirection.UpLeft;
	}

	public static OctaDirection Flip(this OctaDirection dir)
	{
		var newDir = dir;
		switch (dir)
		{
			case OctaDirection.Left: { newDir = OctaDirection.Right; break; }
			case OctaDirection.Right: { newDir = OctaDirection.Left; break; }
			case OctaDirection.UpLeft: { newDir = OctaDirection.DownRight; break; }
			case OctaDirection.UpRight: { newDir = OctaDirection.DownLeft; break; }
			case OctaDirection.Up: { newDir = OctaDirection.Down; break; }
			case OctaDirection.DownLeft: { newDir = OctaDirection.UpRight; break; }
			case OctaDirection.DownRight: { newDir = OctaDirection.UpLeft; break; }
			case OctaDirection.Down: { newDir = OctaDirection.Up; break; }
		}

		return newDir;
	}

	public static OctaDirection Inverse(this OctaDirection dir)
	{
		return dir.Flip();
	}
}