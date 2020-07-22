namespace RogueLike
{
	public static class Functions
	{
		/// <summary>
		/// L-shaped Asymptotic function curve.
		/// </summary>
		public static float LShapedAsymptoticFunction(float a, float b, float x)
		{
			//Asymptotic function, with an L-shaped curve
			return 0.5f * (a + (b / x));
		}
	}
}
