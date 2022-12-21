public static class DebugUtility
{
	private static UnityEngine.Color[] debugColors = new UnityEngine.Color[]
	{
			new UnityEngine.Color(1.0f, 1.0f, 0.0f, 1.0f),
			new UnityEngine.Color(0.4f, 1.0f, 0.2f, 1.0f),
			new UnityEngine.Color(0.0f, 1.0f, 1.0f, 1.0f),
			new UnityEngine.Color(0.6f, 0.4f, 1.0f, 1.0f),
			new UnityEngine.Color(1.0f, 0.0f, 0.4f, 1.0f),
	};

	public static UnityEngine.Color GetDebugColor(int index)
	{
		return debugColors[index % debugColors.Length];
	}
}