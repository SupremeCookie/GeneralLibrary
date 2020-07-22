using UnityEngine;

namespace RogueLike
{
	public static class DotProduct
	{
		public static float DotProductNormalized(Vector2 input, Vector2 input2)
		{
			input = input.normalized;
			input2 = input2.normalized;

			return CalculateDotProduct(input, input2);
		}

		public static float CalculateDotProduct(Vector2 input, Vector2 input2)
		{
			return (input.x * input2.x) + (input.y * input2.y);
		}
	}
}
