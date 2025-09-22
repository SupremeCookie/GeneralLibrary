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

	public static Color MoveTowards(this Color color, Color target, float deltaTime, float duration, out bool isFinished)
	{
		float additionTime = 1.0f / duration * deltaTime;
		Color delta = new Color(target.r - color.r, target.g - color.g, target.b - color.b, target.a - color.a);

		delta.r = Mathf.Abs(delta.r) > 0.01f ? delta.r : 0f;
		delta.g = Mathf.Abs(delta.g) > 0.01f ? delta.g : 0f;
		delta.b = Mathf.Abs(delta.b) > 0.01f ? delta.b : 0f;
		delta.a = Mathf.Abs(delta.a) > 0.01f ? delta.a : 0f;

		Color changedColor = color + delta.Multiply(additionTime);

		isFinished = Mathf.Abs(delta.r) < 0.01f && Mathf.Abs(delta.g) < 0.01f && Mathf.Abs(delta.b) < 0.01f && Mathf.Abs(delta.a) < 0.01f;

		//Debug.Log($"AdditionTime: {additionTime},  delta: {delta},  color: {color},  target: {target},   {changedColor},   duration: {duration}");
		return changedColor;
	}

	public static Color Multiply(this Color target, float amount)
	{
		target.r *= amount;
		target.g *= amount;
		target.b *= amount;
		target.a *= amount;

		return target;
	}

	public static Color Copy(this Color color)
	{
		return new Color(color.r, color.g, color.b, color.a);
	}

	// https://stackoverflow.com/a/20820649/6208164
	// https://www.w3.org/WAI/GL/wiki/Relative_luminance
	// Note DK: desaturation sits between 0 and 1, where 0 returns the original color
	public static Color Desaturate(this Color color, float desaturation)
	{
		var r = color.r;
		var g = color.g;
		var b = color.b;

		float f = desaturation;
		float L = 0.3f * r + 0.6f * g + 0.1f * b;
		color.r = r + f * (L - r);
		color.g = g + f * (L - g);
		color.b = b + f * (L - b);

		return color;
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