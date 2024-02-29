using UnityEngine;

public partial class Utility
{
	public static Gradient CreateDefaultGradient()
	{
		var gradient = new Gradient();
		gradient.colorKeys = new GradientColorKey[]
		{
				new GradientColorKey{ color = Color.black, time = 0.0f, },
				new GradientColorKey{ color = Color.black, time = 1.0f, },
		};

		gradient.alphaKeys = new GradientAlphaKey[]
		{
				new GradientAlphaKey{ alpha = 1.0f, time = 0.0f, },
				new GradientAlphaKey{ alpha = 1.0f, time = 1.0f, },
		};

		gradient.mode = GradientMode.Fixed;

		return gradient;
	}

	public static void KeepBiggestColor(Color first, Color second, ref Color result)
	{
		result.r = Mathf.Max(first.r, second.r);
		result.g = Mathf.Max(first.g, second.g);
		result.b = Mathf.Max(first.b, second.b);
		result.a = Mathf.Max(first.a, second.a);
	}

	private static Color[] randomColors = new Color[]
	{
		new Color(1.0f, 0.0f, 0.0f),
		new Color(1.0f, 0.33f, 0.0f),
		new Color(1.0f, 0.66f, 0.0f),
		new Color(0.0f, 1.0f, 0.0f),
		new Color(0.0f, 1.0f, 0.33f),
		new Color(0.0f, 1.0f, 0.66f),
		new Color(0.0f, 0.0f, 1.0f),
		new Color(0.33f, 0.0f, 1.0f),
		new Color(0.66f, 0.0f, 1.0f),
		new Color(0.0f, 0.0f, 1.0f),
		new Color(0.0f, 0.33f, 1.0f),
		new Color(0.0f, 0.66f, 1.0f),
	};

	public static Color GetRandomColor(int i)
	{
		return randomColors[i % randomColors.Length];
	}
}